using System;
using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double.Distributions
{
    public sealed class BetaA2B2 : Distribution
    {
        private const double ALPHA = 2;
        private const double BETA = 2;
        private const double CONSTANT = 4;

        public BetaA2B2(IRandom rng) : base(rng)
        {
        }
        
        protected override double ShapeFunction(double x) => CONSTANT * Math.Pow(x, ALPHA - 1) * Math.Pow(1 - x, BETA - 1);
    }
}