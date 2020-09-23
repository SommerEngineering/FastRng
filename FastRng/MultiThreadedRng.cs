using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace FastRng
{
    public sealed class MultiThreadedRng : IRandom
    {
        #if DEBUG
            private const int CAPACITY_RANDOM_NUMBERS_4_SOURCE = 10_000;
        #else
            private const int CAPACITY_RANDOM_NUMBERS_4_SOURCE = 16_000_000;
        #endif
        
        private readonly System.Random rng = new System.Random();
        private readonly CancellationTokenSource producerToken = new CancellationTokenSource();
        
        private readonly Thread producerRandom1;
        private readonly Thread producerRandom2;

        private readonly Channel<double> channelRandom = Channel.CreateBounded<double>(new BoundedChannelOptions(CAPACITY_RANDOM_NUMBERS_4_SOURCE)
        {
            FullMode = BoundedChannelFullMode.Wait,
            SingleReader = false,
            SingleWriter = false,
        });

        #region Constructors

        public MultiThreadedRng()
        {
            this.producerRandom1 = new Thread(() => MultiThreadedRng.RandomProducer(this.rng, this.channelRandom.Writer, this.producerToken.Token)) {IsBackground = true};
            this.producerRandom2 = new Thread(() => MultiThreadedRng.RandomProducer(this.rng, this.channelRandom.Writer, this.producerToken.Token)) {IsBackground = true};
            this.producerRandom1.Start();
            this.producerRandom2.Start();
        }

        public MultiThreadedRng(int seed)
        {
            this.rng = new Random(seed);
            
            this.producerRandom1 = new Thread(() => MultiThreadedRng.RandomProducer(this.rng, this.channelRandom.Writer, this.producerToken.Token)) {IsBackground = true};
            this.producerRandom2 = new Thread(() => MultiThreadedRng.RandomProducer(this.rng, this.channelRandom.Writer, this.producerToken.Token)) {IsBackground = true};
            
            this.producerRandom1.Start();
            this.producerRandom2.Start();
        }

        #endregion
        
        [ExcludeFromCodeCoverage]
        private static async void RandomProducer(System.Random random, ChannelWriter<double> channelWriter, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                //
                // We using double as basis for anything. That's what .NET does internally as well, cf. https://github.com/dotnet/runtime/blob/6072e4d3a7a2a1493f514cdf4be75a3d56580e84/src/libraries/System.Private.CoreLib/src/System/Random.cs.
                // random.NextDouble() returns Sample(). Next(min, max) uses GetSampleForLargeRange().
                // Thus, we re-implement GetSampleForLargeRange() and use its numbers as source for everything. 
                //
                
                var buffer = new double[CAPACITY_RANDOM_NUMBERS_4_SOURCE];
                for (var n = 0; n < buffer.Length && !cancellationToken.IsCancellationRequested; n++)
                {
                    #region Re-implementation of GetSampleForLargeRange() method of .NET

                    var result = random.Next(); // Notice: random.Next() is identical to InternalSample()
                    var negative = random.Next() % 2 == 0; // Notice: random.Next() is identical to InternalSample()
                    if (negative)
                        result = -result;
                    
                    double d = result;
                    d += (int.MaxValue - 1); // get a number in range [0 .. 2 * Int32MaxValue - 1)
                    d /= 2 * (uint)int.MaxValue - 1;

                    #endregion
                    
                    buffer[n] = d;
                }

                for (var n = 0; n < buffer.Length && !cancellationToken.IsCancellationRequested; n++) 
                    await channelWriter.WriteAsync(buffer[n], cancellationToken);
            }
        }

        #region Implementing interface

        public async Task<uint> NextNumber(uint rangeStart, uint rangeEnd, CancellationToken cancel = default(CancellationToken))
        {
            var range = rangeEnd - rangeStart;
            return (uint) ((await this.channelRandom.Reader.ReadAsync(cancel) * range) + rangeStart);
        }

        public async Task<ulong> NextNumber(ulong rangeStart, ulong rangeEnd, CancellationToken cancel = default(CancellationToken))
        {
            var range = rangeEnd - rangeStart;
            return (ulong) ((await this.channelRandom.Reader.ReadAsync(cancel) * range) + rangeStart);
        }

        public async Task<float> NextNumber(float rangeStart, float rangeEnd, CancellationToken cancel = default(CancellationToken))
        {
            var range = rangeEnd - rangeStart;
            return (float) ((await this.channelRandom.Reader.ReadAsync(cancel) * range) + rangeStart);
        }

        public void StopProducer() => this.producerToken.Cancel();

        #endregion
    }
}