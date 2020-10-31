using System;

namespace FastRng.Float.Distributions
{
    public sealed class ChiSquareK1 : Distribution
    {
        private const float K = 1.0f;
        private const float K_HALF = K * 0.5f;
        private const float K_HALF_MINUS_ONE = K_HALF - 1.0f;
        private const float CONSTANT = 0.252f;

        private static readonly float DIVISOR;
        
        static ChiSquareK1()
        {
            var twoToTheKHalf = MathF.Pow(2f, K_HALF);
            var gammaKHalf = MathTools.Gamma(K_HALF);
            DIVISOR = twoToTheKHalf * gammaKHalf;
        }

        protected override float ShapeFunction(float x) => CONSTANT * ((MathF.Pow(x, K_HALF_MINUS_ONE) * MathF.Exp(-x * 0.5f)) / DIVISOR);
    }
}