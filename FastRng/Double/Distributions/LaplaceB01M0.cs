using System;
using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double.Distributions
{
    public sealed class LaplaceB01M0 : Distribution
    {
        private const double B = 0.1;
        private const double MU = 0.0;
        private const double CONSTANT = 0.221034183615129;
        
        private static readonly double FACTOR_LEFT;
        
        static LaplaceB01M0()
        {
            FACTOR_LEFT = CONSTANT / (2.0d * B);
        }
        
        public LaplaceB01M0(IRandom rng) : base(rng)
        {
        }

        protected override double ShapeFunction(double x) => FACTOR_LEFT * Math.Exp(-Math.Abs(x - MU) / B);
    }
}