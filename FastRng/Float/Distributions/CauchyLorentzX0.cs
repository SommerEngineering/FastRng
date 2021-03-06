using System;

namespace FastRng.Float.Distributions
{
    public sealed class CauchyLorentzX0 : Distribution
    {
        private const float CONSTANT = 0.31f;
        private const float SCALE = 0.1f;
        private const float MEDIAN = 0.0f;

        public CauchyLorentzX0(IRandom rng) : base(rng)
        {
        }
        
        protected override float ShapeFunction(float x) => CONSTANT * (1.0f / (MathF.PI * SCALE)) * ((SCALE * SCALE) / (MathF.Pow(x - MEDIAN, 2f) + (SCALE * SCALE)));
    }
}