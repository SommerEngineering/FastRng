using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Float.Distributions
{
    public interface IDistribution
    {
        public IRandom Random { get; set; }

        public ValueTask<float> GetDistributedValue(CancellationToken token);
    }
}