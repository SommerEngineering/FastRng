using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double.Distributions
{
    public sealed class NormalS02M05 : Distribution
    {
        private const double SQRT_2_PI = 2.506628275;
        private const double STDDEV = 0.2;
        private const double MEAN = 0.5;

        protected override double ShapeFunction(double x) => 1.0 / (STDDEV * SQRT_2_PI) * Math.Exp(-0.5 * Math.Pow((x - MEAN) / STDDEV, 2.0));
    }
}