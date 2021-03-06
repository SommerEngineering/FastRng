using System;

namespace FastRng.Float.Distributions
{
    public sealed class ExponentialLa5 : Distribution
    {
        private const float LAMBDA = 5.0f;
        private const float CONSTANT = 0.2103f;

        public ExponentialLa5(IRandom rng) : base(rng)
        {
        }
        
        protected override float ShapeFunction(float x) => CONSTANT * LAMBDA * MathF.Exp(-LAMBDA * x);
    }
}