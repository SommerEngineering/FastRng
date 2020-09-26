using System;
using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double.Distributions
{
    public sealed class Cauchy : IDistribution
    {
        private double scale = 1.0;
        
        public IRandom Random { get; set; }
        
        public double Scale
        {
            get => this.scale;
            set
            {
                if(value <= 0.0)
                    throw new ArgumentOutOfRangeException(message: "Scale must be greater than 0", null);
                
                this.scale = value;
            }
        }

        public double Median { get; set; } = 0.0;

        public async ValueTask<double> GetDistributedValue(CancellationToken token)
        {
            if (this.Random == null)
                return System.Double.NaN;

            var value = await this.Random.GetUniform(token);
            return this.Median + scale * Math.Tan(Math.PI * (value - 0.5));
        }
    }
}