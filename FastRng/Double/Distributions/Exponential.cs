using System;
using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double.Distributions
{
    public sealed class Exponential : IDistribution
    {
        private double mean = 1;
        
        public IRandom Random { get; set; }

        public double Mean
        {
            get => this.mean;
            set
            {
                if(value <= 0)
                    throw new ArgumentOutOfRangeException(message: "Mean must be greater than 0", null);
                
                this.mean = value;
            }
        }

        public async ValueTask<double> GetDistributedValue(CancellationToken token)
        {
            if (this.Random == null)
                return 0.0;

            if(this.Mean == 1)
                return -Math.Log(await this.Random.GetUniform(token));
            else
                return this.Mean * -Math.Log(await this.Random.GetUniform(token));
        }
    }
}