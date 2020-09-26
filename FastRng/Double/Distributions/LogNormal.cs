using System;
using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double.Distributions
{
    public sealed class LogNormal : IDistribution
    {
        private double sigma = 1.0;
        public IRandom Random { get; set; }

        public double Mu { get; set; } = 0.0;

        public double Sigma
        {
            get => this.sigma;
            set
            {
                if(value <= 0.0)
                    throw new ArgumentOutOfRangeException(message: "Sigma must be greater than 0", null);
                
                this.sigma = value;
            }
        }

        public async ValueTask<double> GetDistributedValue(CancellationToken token)
        {
            if (this.Random == null)
                return System.Double.NaN;

            var normal = await this.Random.NextNumber(new Normal {Mean = this.Mu, StandardDeviation = this.Sigma}, token);
            return Math.Exp(normal);
        }
    }
}