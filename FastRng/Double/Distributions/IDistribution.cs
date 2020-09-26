using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double.Distributions
{
    public interface IDistribution
    {
        public IRandom Random { get; set; }

        public ValueTask<double> GetDistributedValue(CancellationToken token);
    }
}