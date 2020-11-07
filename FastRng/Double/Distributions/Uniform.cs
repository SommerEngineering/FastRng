using System;
using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double.Distributions
{
    public sealed class Uniform : IDistribution
    {
        private readonly IRandom rng;
        
        public Uniform(IRandom rng)
        {
            if (rng == null)
                throw new ArgumentNullException(nameof(rng), "An IRandom implementation is needed.");
            
            this.rng = rng;
        }
        
        public async ValueTask<double> GetDistributedValue(CancellationToken token = default) => await this.rng.GetUniform(token);
        
        public async ValueTask<uint> NextNumber(uint rangeStart, uint rangeEnd, CancellationToken cancel = default)
        {
            if (rangeStart > rangeEnd)
            {
                var tmp = rangeStart;
                rangeStart = rangeEnd;
                rangeEnd = tmp;
            }
            
            var range = rangeEnd - rangeStart;
            var distributedValue = await this.GetDistributedValue(cancel);
            return (uint) ((distributedValue * range) + rangeStart);
        }

        public async ValueTask<ulong> NextNumber(ulong rangeStart, ulong rangeEnd, CancellationToken cancel = default(CancellationToken))
        {
            if (rangeStart > rangeEnd)
            {
                var tmp = rangeStart;
                rangeStart = rangeEnd;
                rangeEnd = tmp;
            }
            
            var range = rangeEnd - rangeStart;
            var distributedValue = await this.GetDistributedValue(cancel);
            return (ulong) ((distributedValue * range) + rangeStart);
        }

        public async ValueTask<double> NextNumber(double rangeStart, double rangeEnd, CancellationToken cancel = default(CancellationToken))
        {
            if (rangeStart > rangeEnd)
            {
                var tmp = rangeStart;
                rangeStart = rangeEnd;
                rangeEnd = tmp;
            }
            
            var range = rangeEnd - rangeStart;
            var distributedValue = await this.GetDistributedValue(cancel);
            return (distributedValue * range) + rangeStart;
        }

        public async ValueTask<double> NextNumber(CancellationToken cancel = default) => await this.NextNumber(0.0, 1.0, cancel);
        
        public async ValueTask<bool> HasDecisionBeenMade(double above, double below = 1, CancellationToken cancel = default)
        {
            var number = await this.NextNumber(cancel);
            return number > above && number < below;
        }
    }
}