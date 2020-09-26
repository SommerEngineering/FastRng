using System;
using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double.Distributions
{
    public sealed class Laplace : IDistribution
    {
        public IRandom Random { get; set; }

        public double Mean { get; set; } = 0.0;

        public double Scale { get; set; } = 1.0;
        
        public async ValueTask<double> GetDistributedValue(CancellationToken token)
        {
            if (this.Random == null)
                return System.Double.NaN;

            var value = await this.Random.GetUniform(token);
            
            if (value < 0.5)
                return this.Mean + this.Scale * Math.Log(2.0 * value);
            else
                return this.Mean - this.Scale * Math.Log(2.0 * (1.0 - value));
        }
    }
}