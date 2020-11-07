using System;

namespace FastRng.Float.Distributions
{
    public sealed class InverseExponentialLa10 : Distribution
    {
        private const float LAMBDA = 10.0f;
        private const float CONSTANT = 4.539992976248453e-06f;

        public InverseExponentialLa10(IRandom rng) : base(rng)
        {
        }
        
        protected override float ShapeFunction(float x) => CONSTANT * LAMBDA * MathF.Exp(LAMBDA * x);
    }
}