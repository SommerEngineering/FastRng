using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double.Distributions
{
    public interface IDistribution
    {
        public ValueTask<double> GetDistributedValue(CancellationToken token);
    }
}