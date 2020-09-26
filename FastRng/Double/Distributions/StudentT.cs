using System;
using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double.Distributions
{
    public sealed class StudentT : IDistribution
    {
        private static readonly IDistribution NORMAL_DISTRIBUTED = new Normal();
        private double degreesOfFreedom = 1.0;
        
        public IRandom Random { get; set; }
        
        public double DegreesOfFreedom
        {
            get => this.degreesOfFreedom;
            set
            {
                if(value <= 0.0)
                    throw new ArgumentOutOfRangeException(message: "DegreesOfFreedom must be greater than 0", null);
                
                this.degreesOfFreedom = value;
            }
        }
        
        public async ValueTask<double> GetDistributedValue(CancellationToken token)
        {
            if (this.Random == null)
                return System.Double.NaN;

            var normal = await this.Random.NextNumber(NORMAL_DISTRIBUTED, token);
            var chiSquare = await this.Random.NextNumber(new ChiSquare {DegreesOfFreedom = this.DegreesOfFreedom}, token);
            return normal / Math.Sqrt(chiSquare / this.DegreesOfFreedom);
        }
    }
}