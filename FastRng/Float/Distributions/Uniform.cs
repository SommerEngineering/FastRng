using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Float.Distributions
{
    public sealed class Uniform : IDistribution
    {
        public IRandom Random { get; set; }
        
        public async ValueTask<float> GetDistributedValue(CancellationToken token = default) => this.Random == null ? float.NaN : await this.Random.GetUniform(token);
    }
}