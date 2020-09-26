using System;
using System.Threading;
using System.Threading.Tasks;
using FastRng.Distributions.Double;

namespace FastRng
{
    public interface IRandom
    {
        public Task<double> GetUniform(CancellationToken cancel = default);
        
        public Task<uint> NextNumber(uint rangeStart, uint rangeEnd, IDistribution distribution, CancellationToken cancel = default);
        
        public Task<ulong> NextNumber(ulong rangeStart, ulong rangeEnd, IDistribution distribution, CancellationToken cancel = default);
        
        public Task<float> NextNumber(float rangeStart, float rangeEnd, IDistribution distribution, CancellationToken cancel = default);

        public void StopProducer();
    }
}