using System;
using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double.Distributions
{
    public sealed class CauchyLorentzX1 : Distribution
    {
        private const double CONSTANT = 0.31;
        private const double SCALE = 0.1;
        private const double MEDIAN = 1.0;

        public CauchyLorentzX1(IRandom rng) : base(rng)
        {
        }
        
        protected override double ShapeFunction(double x) => CONSTANT * (1.0 / (Math.PI * SCALE)) * ((SCALE * SCALE) / (Math.Pow(x - MEDIAN, 2) + (SCALE * SCALE)));
    }
}