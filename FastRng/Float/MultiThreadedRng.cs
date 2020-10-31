using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using FastRng.Float.Distributions;

namespace FastRng.Float
{
    /// <summary>
    /// This class uses the George Marsaglia's MWC algorithm. The algorithm's implementation based loosely on John D.
    /// Cook's (johndcook.com) implementation (https://www.codeproject.com/Articles/25172/Simple-Random-Number-Generation).
    /// Thanks John for your work.
    /// </summary>
    public sealed class MultiThreadedRng : IRandom
    {
        #if DEBUG
            private const int CAPACITY_RANDOM_NUMBERS_4_SOURCE = 10_000;
        #else
            private const int CAPACITY_RANDOM_NUMBERS_4_SOURCE = 16_000_000;
        #endif
        
        private readonly CancellationTokenSource producerTokenSource = new CancellationTokenSource();
        private readonly object syncUintGenerators = new object();
        private readonly object syncUniformDistributedFloatGenerators = new object();
        private readonly Thread[] producerRandomUint = new Thread[2];
        private readonly Thread[] producerRandomUniformDistributedFloat = new Thread[2];
        
        private uint mW;
        private uint mZ;

        private readonly Channel<uint> channelRandomUint = Channel.CreateBounded<uint>(new BoundedChannelOptions(CAPACITY_RANDOM_NUMBERS_4_SOURCE)
        {
            FullMode = BoundedChannelFullMode.Wait,
            SingleReader = false,
            SingleWriter = false,
        });
        
        private readonly Channel<float> channelRandomUniformDistributedFloat = Channel.CreateBounded<float>(new BoundedChannelOptions(CAPACITY_RANDOM_NUMBERS_4_SOURCE)
        {
            FullMode = BoundedChannelFullMode.Wait,
            SingleReader = false,
            SingleWriter = false,
        });

        #region Constructors

        public MultiThreadedRng()
        {
            //
            // Initialize the mW and mZ by using
            // the system's time.
            //
            var now = DateTime.Now;
            var ticks = now.Ticks;
            this.mW = (uint) (ticks >> 16);
            this.mZ = (uint) (ticks % 4294967296);
            this.StartProducerThreads(deterministic: false);
        }

        public MultiThreadedRng(uint seedU)
        {
            this.mW = seedU;
            this.mZ = 362436069;
            this.StartProducerThreads(deterministic: true);
        }
        
        public MultiThreadedRng(uint seedU, uint seedV)
        {
            this.mW = seedU;
            this.mZ = seedV;
            this.StartProducerThreads(deterministic: true);
        }

        private void StartProducerThreads(bool deterministic = false)
        {
            this.producerRandomUint[0] = new Thread(() => this.RandomProducerUint(this.channelRandomUint.Writer, this.producerTokenSource.Token)) {IsBackground = true};
            this.producerRandomUint[1] = new Thread(() => this.RandomProducerUint(this.channelRandomUint.Writer, this.producerTokenSource.Token)) {IsBackground = true};
            this.producerRandomUint[0].Start();
            
            if(!deterministic)
                this.producerRandomUint[1].Start();
            
            this.producerRandomUniformDistributedFloat[0] = new Thread(() => this.RandomProducerUniformDistributedFloat(this.channelRandomUint.Reader, channelRandomUniformDistributedFloat.Writer, this.producerTokenSource.Token)) {IsBackground = true};
            this.producerRandomUniformDistributedFloat[1] = new Thread(() => this.RandomProducerUniformDistributedFloat(this.channelRandomUint.Reader, channelRandomUniformDistributedFloat.Writer, this.producerTokenSource.Token)) {IsBackground = true};
            this.producerRandomUniformDistributedFloat[0].Start();
            
            if(!deterministic)
                this.producerRandomUniformDistributedFloat[1].Start();
        }

        #endregion

        #region Producers

        [ExcludeFromCodeCoverage]
        private async void RandomProducerUint(ChannelWriter<uint> channelWriter, CancellationToken cancellationToken)
        {
            try
            {
                var buffer = new uint[CAPACITY_RANDOM_NUMBERS_4_SOURCE];
                while (!cancellationToken.IsCancellationRequested)
                {
                    lock (syncUintGenerators)
                    {
                        for (var n = 0; n < buffer.Length && !cancellationToken.IsCancellationRequested; n++)
                        {
                            this.mZ = 36_969 * (this.mZ & 65_535) + (this.mZ >> 16);
                            this.mW = 18_000 * (this.mW & 65_535) + (this.mW >> 16);
                            buffer[n] = (this.mZ << 16) + this.mW;
                        }
                    }

                    for (var n = 0; n < buffer.Length && !cancellationToken.IsCancellationRequested; n++) 
                        await channelWriter.WriteAsync(buffer[n], cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }
        
        [ExcludeFromCodeCoverage]
        private async void RandomProducerUniformDistributedFloat(ChannelReader<uint> channelReaderUint, ChannelWriter<float> channelWriter, CancellationToken cancellationToken)
        {
            try
            {
                var buffer = new float[CAPACITY_RANDOM_NUMBERS_4_SOURCE];
                var randomUint = new uint[CAPACITY_RANDOM_NUMBERS_4_SOURCE];
                while (!cancellationToken.IsCancellationRequested)
                {
                    for (var n = 0; n < randomUint.Length; n++)
                        randomUint[n] = await channelReaderUint.ReadAsync(cancellationToken);

                    lock (syncUniformDistributedFloatGenerators)
                        for (var n = 0; n < buffer.Length && !cancellationToken.IsCancellationRequested; n++)
                            buffer[n] = (randomUint[n] + 1.0f) * 2.328306435454494e-10f; // 2.328 => 1/(2^32 + 2)

                    for (var n = 0; n < buffer.Length && !cancellationToken.IsCancellationRequested; n++) 
                        await channelWriter.WriteAsync(buffer[n], cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        #endregion

        #region Implementing interface

        public async ValueTask<float> GetUniform(CancellationToken cancel = default)
        {
            try
            {
                return await this.channelRandomUniformDistributedFloat.Reader.ReadAsync(cancel);
            }
            catch (OperationCanceledException)
            {
                return float.NaN;
            }
        }

        public async ValueTask<uint> NextNumber(uint rangeStart, uint rangeEnd, IDistribution distribution, CancellationToken cancel = default)
        {
            if (rangeStart > rangeEnd)
            {
                var tmp = rangeStart;
                rangeStart = rangeEnd;
                rangeEnd = tmp;
            }
            
            var range = rangeEnd - rangeStart;
            distribution.Random ??= this;
            
            var distributedValue = await distribution.GetDistributedValue(cancel);
            return (uint) ((distributedValue * range) + rangeStart);
        }

        public async ValueTask<ulong> NextNumber(ulong rangeStart, ulong rangeEnd, IDistribution distribution, CancellationToken cancel = default(CancellationToken))
        {
            if (rangeStart > rangeEnd)
            {
                var tmp = rangeStart;
                rangeStart = rangeEnd;
                rangeEnd = tmp;
            }
            
            var range = rangeEnd - rangeStart;
            distribution.Random = this;
            
            var distributedValue = await distribution.GetDistributedValue(cancel);
            return (ulong) ((distributedValue * range) + rangeStart);
        }

        public async ValueTask<float> NextNumber(float rangeStart, float rangeEnd, IDistribution distribution, CancellationToken cancel = default(CancellationToken))
        {
            if (rangeStart > rangeEnd)
            {
                var tmp = rangeStart;
                rangeStart = rangeEnd;
                rangeEnd = tmp;
            }
            
            var range = rangeEnd - rangeStart;
            distribution.Random = this;
            
            var distributedValue = await distribution.GetDistributedValue(cancel);
            return (distributedValue * range) + rangeStart;
        }

        public async ValueTask<float> NextNumber(IDistribution distribution, CancellationToken cancel = default) => await this.NextNumber(0.0f, 1.0f, distribution, cancel);

        public void StopProducer() => this.producerTokenSource.Cancel();

        #endregion
    }
}