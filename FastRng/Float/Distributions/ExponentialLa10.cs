using System;

namespace FastRng.Float.Distributions
{
    public sealed class ExponentialLa10 : Distribution
    {
        private const float LAMBDA = 10.0f;
        private const float CONSTANT = 0.1106f;

        public ExponentialLa10(IRandom rng) : base(rng)
        {
        }
        
        protected override float ShapeFunction(float x) => CONSTANT * LAMBDA * MathF.Exp(-LAMBDA * x);
    }
}