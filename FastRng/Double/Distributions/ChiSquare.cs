using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double.Distributions
{
    public sealed class ChiSquare : IDistribution
    {
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
                return 0.0;

            return await this.Random.NextNumber(new Gamma{ Shape = 0.5 * this.DegreesOfFreedom, Scale = 2.0 }, token);
        }
    }
}