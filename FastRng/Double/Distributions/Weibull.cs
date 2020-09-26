using System;
using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double.Distributions
{
    public sealed class Weibull : IDistribution
    {
        private double shape = 1.0;
        private double scale = 1.0;

        public IRandom Random { get; set; }

        public double Shape
        {
            get => this.shape;
            set
            {
                if(value <= 0.0)
                    throw new ArgumentOutOfRangeException(message: "Shape must be greater than 0", null);
                
                this.shape = value;
            }
        }

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

        public async ValueTask<double> GetDistributedValue(CancellationToken token = default)
        {
            if (this.Random == null)
                return System.Double.NaN;

            var value = await this.Random.GetUniform(token);
            return this.Scale * Math.Pow(-Math.Log(value), 1.0 / this.Shape);
        }
    }
}