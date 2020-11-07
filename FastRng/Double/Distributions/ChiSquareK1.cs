using System;
using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double.Distributions
{
    public sealed class ChiSquareK1 : Distribution
    {
        private const double K = 1.0;
        private const double K_HALF = K * 0.5d;
        private const double K_HALF_MINUS_ONE = K_HALF - 1.0d;
        private const double CONSTANT = 0.252;

        private static readonly double DIVISOR;
        
        static ChiSquareK1()
        {
            var twoToTheKHalf = Math.Pow(2, K_HALF);
            var gammaKHalf = MathTools.Gamma(K_HALF);
            DIVISOR = twoToTheKHalf * gammaKHalf;
        }

        public ChiSquareK1(IRandom rng) : base(rng)
        {
        }
        
        protected override double ShapeFunction(double x) => CONSTANT * ((Math.Pow(x, K_HALF_MINUS_ONE) * Math.Exp(-x * 0.5d)) / DIVISOR);
    }
}