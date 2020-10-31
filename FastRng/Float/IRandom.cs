using System.Threading;
using System.Threading.Tasks;
using FastRng.Float.Distributions;

namespace FastRng.Float
{
    public interface IRandom
    {
        public ValueTask<float> GetUniform(CancellationToken cancel = default);
        
        public ValueTask<uint> NextNumber(uint rangeStart, uint rangeEnd, IDistribution distribution, CancellationToken cancel = default);
        
        public ValueTask<ulong> NextNumber(ulong rangeStart, ulong rangeEnd, IDistribution distribution, CancellationToken cancel = default);
        
        public ValueTask<float> NextNumber(float rangeStart, float rangeEnd, IDistribution distribution, CancellationToken cancel = default);
        
        public ValueTask<float> NextNumber(IDistribution distribution, CancellationToken cancel = default);

        public void StopProducer();
    }
}