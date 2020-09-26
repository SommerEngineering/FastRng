using System.Threading;
using System.Threading.Tasks;
using FastRng.Double.Distributions;

namespace FastRng.Double
{
    public interface IRandom
    {
        public ValueTask<double> GetUniform(CancellationToken cancel = default);
        
        public ValueTask<uint> NextNumber(uint rangeStart, uint rangeEnd, IDistribution distribution, CancellationToken cancel = default);
        
        public ValueTask<ulong> NextNumber(ulong rangeStart, ulong rangeEnd, IDistribution distribution, CancellationToken cancel = default);
        
        public ValueTask<double> NextNumber(double rangeStart, double rangeEnd, IDistribution distribution, CancellationToken cancel = default);
        
        public ValueTask<double> NextNumber(IDistribution distribution, CancellationToken cancel = default);

        public void StopProducer();
    }
}