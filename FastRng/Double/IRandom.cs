using System;
using System.Threading;
using System.Threading.Tasks;
using FastRng.Double.Distributions;

namespace FastRng.Double
{
    public interface IRandom : IDisposable
    {
        public ValueTask<double> GetUniform(CancellationToken cancel = default);
    }
}