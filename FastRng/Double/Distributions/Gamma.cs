using System;
using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double.Distributions
{
    public sealed class Gamma : IDistribution
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
        
        public async ValueTask<double> GetDistributedValue(CancellationToken token)
        {
            if (this.Random == null)
                return 0.0;
            
            // Implementation based on "A Simple Method for Generating Gamma Variables"
            // by George Marsaglia and Wai Wan Tsang.  ACM Transactions on Mathematical Software
            // Vol 26, No 3, September 2000, pages 363-372.
            
            if (shape >= 1.0)
            {
                var distNormal = new Normal();
                var d = shape - 1.0 / 3.0;
                var c = 1.0 / Math.Sqrt(9.0 * d);
                while(true)
                {
                    double x, v;
                    do
                    {
                        x = await this.Random.NextNumber(0, 1, distNormal, token);
                        v = 1.0 + c * x;
                    }
                    while (v <= 0.0);
                    
                    v = v * v * v;
                    var u = await this.Random.GetUniform(token);
                    var xSquared = x * x;
                    
                    if (u < 1.0 - 0.0331 * xSquared * xSquared || Math.Log(u) < 0.5 * xSquared + d * (1.0 - v + Math.Log(v)))
                        return this.Scale * d * v;
                }
            }
            else
            {
                var dist = new Gamma{ Scale = 1, Shape = 1 + this.Shape};
                
                var g = await this.Random.NextNumber(0.0f, 1.0f, dist, token); // TODO: Use double
                var w = await this.Random.GetUniform(token);
                return this.Scale * g * Math.Pow(w, 1.0 / this.Shape);
            }
        }
    }
}