using System;

namespace FastRng.Float.Distributions
{
    public sealed class BetaA2B2 : Distribution
    {
        private const float ALPHA = 2f;
        private const float BETA = 2f;
        private const float CONSTANT = 4f;

        protected override float ShapeFunction(float x) => CONSTANT * MathF.Pow(x, ALPHA - 1f) * MathF.Pow(1f - x, BETA - 1f);
    }
}