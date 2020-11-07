using System;

namespace FastRng.Float.Distributions
{
    public sealed class WeibullK05La1 : Distribution
    {
        private const float K = 0.5f;
        private const float LAMBDA = 1.0f;
        private const float CONSTANT = 0.221034183615129f;

        public WeibullK05La1(IRandom rng) : base(rng)
        {
        }
        
        protected override float ShapeFunction(float x) => CONSTANT * ( (K / LAMBDA) * MathF.Pow(x / LAMBDA, K - 1.0f) * MathF.Exp(-MathF.Pow(x/LAMBDA, K)));
    }
}