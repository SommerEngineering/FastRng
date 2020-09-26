using System;
using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double.Distributions
{
    public sealed class InverseGamma : IDistribution
    {
        private double shape = 1.0;

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

        public double Scale { get; set; } = 1.0;
        
        public async ValueTask<double> GetDistributedValue(CancellationToken token = default)
        {
            if (this.Random == null)
                return System.Double.NaN;
            
            var gammaDist = new Gamma{ Shape = this.Shape, Scale = 1.0 / this.Scale };
            return 1.0 / await this.Random.NextNumber(gammaDist, token);
        }
    }
}