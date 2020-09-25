using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using FastRng.Distributions;

namespace FastRng
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
        
        private readonly CancellationTokenSource producerToken = new CancellationTokenSource();
        private readonly object syncUintGenerators = new object();
        private readonly object syncUniformDistributedDoubleGenerators = new object();
        private readonly Thread[] producerRandomUint = new Thread[2];
        private readonly Thread[] producerRandomUniformDistributedDouble = new Thread[2];
        
        private uint mW;
        private uint mZ;

        private readonly Channel<uint> channelRandomUint = Channel.CreateBounded<uint>(new BoundedChannelOptions(CAPACITY_RANDOM_NUMBERS_4_SOURCE)
        {
            FullMode = BoundedChannelFullMode.Wait,
            SingleReader = false,
            SingleWriter = false,
        });
        
        private readonly Channel<double> channelRandomUniformDistributedDouble = Channel.CreateBounded<double>(new BoundedChannelOptions(CAPACITY_RANDOM_NUMBERS_4_SOURCE)
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
            this.StartProducerThreads();
        }

        public MultiThreadedRng(uint seedU)
        {
            this.mW = seedU;
            this.mZ = 362436069;
            this.StartProducerThreads();
        }
        
        public MultiThreadedRng(uint seedU, uint seedV)
        {
            this.mW = seedU;
            this.mZ = seedV;
            this.StartProducerThreads();
        }

        private void StartProducerThreads()
        {
            this.producerRandomUint[0] = new Thread(() => this.RandomProducerUint(this.channelRandomUint.Writer, this.producerToken.Token)) {IsBackground = true};
            this.producerRandomUint[1] = new Thread(() => this.RandomProducerUint(this.channelRandomUint.Writer, this.producerToken.Token)) {IsBackground = true};
            this.producerRandomUint[0].Start();
            this.producerRandomUint[1].Start();
            
            this.producerRandomUniformDistributedDouble[0] = new Thread(() => this.RandomProducerUniformDistributedDouble(this.channelRandomUint.Reader, channelRandomUniformDistributedDouble.Writer, this.producerToken.Token)) {IsBackground = true};
            this.producerRandomUniformDistributedDouble[1] = new Thread(() => this.RandomProducerUniformDistributedDouble(this.channelRandomUint.Reader, channelRandomUniformDistributedDouble.Writer, this.producerToken.Token)) {IsBackground = true};
            this.producerRandomUniformDistributedDouble[0].Start();
            this.producerRandomUniformDistributedDouble[1].Start();
        }

        #endregion

        #region Producers

        [ExcludeFromCodeCoverage]
        private async void RandomProducerUint(ChannelWriter<uint> channelWriter, CancellationToken cancellationToken)
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
        
        [ExcludeFromCodeCoverage]
        private async void RandomProducerUniformDistributedDouble(ChannelReader<uint> channelReaderUint, ChannelWriter<double> channelWriter, CancellationToken cancellationToken)
        {
            var buffer = new double[CAPACITY_RANDOM_NUMBERS_4_SOURCE];
            var randomUint = new uint[CAPACITY_RANDOM_NUMBERS_4_SOURCE];
            while (!cancellationToken.IsCancellationRequested)
            {
                for (var n = 0; n < randomUint.Length; n++)
                    randomUint[n] = await channelReaderUint.ReadAsync(cancellationToken);
                
                lock (syncUniformDistributedDoubleGenerators)
                    for (var n = 0; n < buffer.Length && !cancellationToken.IsCancellationRequested; n++)
                        buffer[n] = (randomUint[n] + 1.0) * 2.328306435454494e-10; // 2.328 => 1/(2^32 + 2)

                for (var n = 0; n < buffer.Length && !cancellationToken.IsCancellationRequested; n++) 
                    await channelWriter.WriteAsync(buffer[n], cancellationToken);
            }
        }

        #endregion

        #region Implementing interface

        public async Task<double> GetUniformDouble(CancellationToken cancel = default) => await this.channelRandomUniformDistributedDouble.Reader.ReadAsync(cancel);

        public async Task<uint> NextNumber(uint rangeStart, uint rangeEnd, IDistribution distribution, CancellationToken cancel = default)
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
            return (uint) ((distributedValue * range) + rangeStart);
        }

        public async Task<ulong> NextNumber(ulong rangeStart, ulong rangeEnd, IDistribution distribution, CancellationToken cancel = default(CancellationToken))
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

        public async Task<float> NextNumber(float rangeStart, float rangeEnd, IDistribution distribution, CancellationToken cancel = default(CancellationToken))
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
            return (float) ((distributedValue * range) + rangeStart);
        }

        public void StopProducer() => this.producerToken.Cancel();

        #endregion
    }
}