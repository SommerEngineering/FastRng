using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double.Distributions
{
    public sealed class ChiSquare : IDistribution
    {
        public IRandom Random { get; set; }

        public double DegreesOfFreedom { get; set; } = 1.0;
        
        public async ValueTask<double> GetDistributedValue(CancellationToken token)
        {
            if (this.Random == null)
                return 0.0;

            return await this.Random.NextNumber(new Gamma{ Shape = 0.5 * this.DegreesOfFreedom, Scale = 2.0 }, token);
        }
    }
}