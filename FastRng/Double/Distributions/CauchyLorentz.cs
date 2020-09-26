using System;
using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double.Distributions
{
    public sealed class CauchyLorentz : IDistribution
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

        public async ValueTask<double> GetDistributedValue(CancellationToken token = default)
        {
            if (this.Random == null)
                return double.NaN;

            var value = await this.Random.GetUniform(token);
            return 1.0 / (Math.PI * this.Scale * (1 + Math.Pow((value - this.Median) / this.Scale, 2)));
        }
    }
}