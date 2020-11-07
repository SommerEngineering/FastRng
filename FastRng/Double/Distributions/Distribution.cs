using System;
using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double.Distributions
{
    public abstract class Distribution : IDistribution
    {
        private readonly ShapeFitter fitter;
        private readonly IRandom random;

        protected Distribution(IRandom rng)
        {
            if (rng == null)
                throw new ArgumentNullException(nameof(rng), "An IRandom implementation is needed.");
                
            this.random = rng;
            this.fitter = new ShapeFitter(this.ShapeFunction, this.random, 100);
        }

        protected abstract double ShapeFunction(double x);
        
        public async ValueTask<double> GetDistributedValue(CancellationToken token = default) => await this.fitter.NextNumber(token);
        
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
    }
}