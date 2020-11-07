using System;
using System.Threading;
using System.Threading.Tasks;
using FastRng.Float.Distributions;

namespace FastRng.Float
{
    public interface IRandom : IDisposable
    {
        public ValueTask<float> GetUniform(CancellationToken cancel = default);
    }
}