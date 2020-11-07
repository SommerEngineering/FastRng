using System;

namespace FastRng.Float.Distributions
{
    public sealed class InverseExponentialLa5 : Distribution
    {
        private const float LAMBDA = 5.0f;
        private const float CONSTANT = 0.001347589399817f;

        public InverseExponentialLa5(IRandom rng) : base(rng)
        {
        }
        
        protected override float ShapeFunction(float x) => CONSTANT * LAMBDA * MathF.Exp(LAMBDA * x);
    }
}