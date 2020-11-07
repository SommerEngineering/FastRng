using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double.Distributions
{
    public interface IDistribution
    {
        public ValueTask<double> GetDistributedValue(CancellationToken token);
        
        public ValueTask<uint> NextNumber(uint rangeStart, uint rangeEnd, CancellationToken cancel = default);
        
        public ValueTask<ulong> NextNumber(ulong rangeStart, ulong rangeEnd, CancellationToken cancel = default);
        
        public ValueTask<double> NextNumber(double rangeStart, double rangeEnd, CancellationToken cancel = default);
        
        public ValueTask<double> NextNumber(CancellationToken cancel = default);

        public ValueTask<bool> HasDecisionBeenMade(double above, double below = 1.0, CancellationToken cancel = default);
    }
}