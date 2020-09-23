using System.Threading;
using System.Threading.Tasks;

namespace FastRng
{
    public interface IRandom
    {
        public Task<uint> NextNumber(uint rangeStart, uint rangeEnd, CancellationToken cancel = default(CancellationToken));
        
        public Task<ulong> NextNumber(ulong rangeStart, ulong rangeEnd, CancellationToken cancel = default(CancellationToken));
        
        public Task<float> NextNumber(float rangeStart, float rangeEnd, CancellationToken cancel = default(CancellationToken));

        public void StopProducer();
    }
}