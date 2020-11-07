using System;

namespace FastRng.Float.Distributions
{
    public sealed class BetaA5B2 : Distribution
    {
        private const float ALPHA = 5f;
        private const float BETA = 2f;
        private const float CONSTANT = 12.2f;

        public BetaA5B2(IRandom rng) : base(rng)
        {
        }
        
        protected override float ShapeFunction(float x) => CONSTANT * MathF.Pow(x, ALPHA - 1f) * MathF.Pow(1f - x, BETA - 1f);
    }
}