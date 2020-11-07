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
    }
}