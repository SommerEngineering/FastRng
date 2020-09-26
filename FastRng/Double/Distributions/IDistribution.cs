using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double.Distributions
{
    public interface IDistribution
    {
        public IRandom Random { get; set; }

        public Task<double> GetDistributedValue(CancellationToken token);
    }
}