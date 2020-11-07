using System;

namespace FastRng.Float.Distributions
{
    public sealed class GammaA5B15 : Distribution
    {
        private const float ALPHA = 5.0f;
        private const float BETA = 15.0f;
        private const float CONSTANT = 0.341344210715475f;

        private static readonly float GAMMA_ALPHA;
        private static readonly float BETA_TO_THE_ALPHA;
        
        static GammaA5B15()
        {
            GAMMA_ALPHA = MathTools.Gamma(ALPHA);
            BETA_TO_THE_ALPHA = MathF.Pow(BETA, ALPHA);
        }

        public GammaA5B15(IRandom rng) : base(rng)
        {
        }
        
        protected override float ShapeFunction(float x) => CONSTANT * ((BETA_TO_THE_ALPHA * MathF.Pow(x, ALPHA - 1.0f) * MathF.Exp(-BETA * x)) / GAMMA_ALPHA);
    }
}