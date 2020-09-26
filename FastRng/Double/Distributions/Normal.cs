using System;
using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double.Distributions
{
    public sealed class Normal : IDistribution
    {
        private double standardDeviation = 1.0;
        
        public IRandom Random { get; set; }

        public double Mean { get; set; } = 0.0;

        public double StandardDeviation
        {
            get => this.standardDeviation;
            set
            {
                if(value <= 0.0)
                    throw new ArgumentOutOfRangeException(message: "Standard deviation must be greater than 0", null);
                
                this.standardDeviation = value;
            }
        }

        public async ValueTask<double> GetDistributedValue(CancellationToken token = default)
        {
            if (this.Random == null)
                return System.Double.NaN;

            var u1 = await this.Random.GetUniform(token);
            var u2 = await this.Random.GetUniform(token);
            var r = Math.Sqrt(-2.0 * Math.Log(u1));
            var theta = 2.0 * Math.PI * u2;
            var value = r * Math.Sin(theta);

            return this.Mean + this.StandardDeviation * value;
        }
    }
}