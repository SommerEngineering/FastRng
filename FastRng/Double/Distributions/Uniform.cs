using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double.Distributions
{
    public sealed class Uniform : IDistribution
    {
        public IRandom Random { get; set; }
        
        public async ValueTask<double> GetDistributedValue(CancellationToken token = default) => this.Random == null ? 0 : await this.Random.GetUniform(token);
    }
}