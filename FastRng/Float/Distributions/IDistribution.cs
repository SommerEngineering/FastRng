using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Float.Distributions
{
    public interface IDistribution
    {
        public ValueTask<float> GetDistributedValue(CancellationToken token);
        
        public ValueTask<uint> NextNumber(uint rangeStart, uint rangeEnd, CancellationToken cancel = default);
        
        public ValueTask<ulong> NextNumber(ulong rangeStart, ulong rangeEnd, CancellationToken cancel = default);
        
        public ValueTask<float> NextNumber(float rangeStart, float rangeEnd, CancellationToken cancel = default);
        
        public ValueTask<float> NextNumber(CancellationToken cancel = default);
    }
}