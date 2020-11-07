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
    }
}