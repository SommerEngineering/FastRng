using System;
using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double.Distributions
{
    public sealed class Beta : IDistribution
    {
        private double a = 1.0;
        private double b = 1.0;
        public IRandom Random { get; set; }

        public double A
        {
            get => this.a;
            set
            {
                if(value <= 0.0)
                    throw new ArgumentOutOfRangeException(message: "Parameter must be greater than 0", null);
                
                this.a = value;
            }
        }

        public double B
        {
            get => this.b;
            set
            {
                if(value <= 0.0)
                    throw new ArgumentOutOfRangeException(message: "Parameter must be greater than 0", null);
                
                this.b = value;
            }
        }

        public async ValueTask<double> GetDistributedValue(CancellationToken token = default)
        {
            if (this.Random == null)
                return double.NaN;
            
            // There are more efficient methods for generating beta samples.
            // However such methods are a little more efficient and much more complicated.
            // For an explanation of why the following method works, see
            // http://www.johndcook.com/distribution_chart.html#gamma_beta
 
            var u = await this.Random.NextNumber(new Gamma{Shape = this.A, Scale = 1.0}, token);
            var v = await this.Random.NextNumber(new Gamma{Shape = this.B, Scale = 1.0}, token);
            return u / (u + v);
        }
    }
}