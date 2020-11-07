using System;

namespace FastRng.Float.Distributions
{
    public sealed class InverseGammaA3B05 : Distribution
    {
        private const float ALPHA = 3.0f;
        private const float BETA = 0.5f;
        private const float CONSTANT = 0.213922656884911f;

        private static readonly float FACTOR_LEFT;
        
        static InverseGammaA3B05()
        {
            var gammaAlpha = MathTools.Gamma(ALPHA);
            var betaToTheAlpha = MathF.Pow(BETA, ALPHA);
            
            FACTOR_LEFT = CONSTANT * (betaToTheAlpha / gammaAlpha);
        }
        
        public InverseGammaA3B05(IRandom rng) : base(rng)
        {
        }

        protected override float ShapeFunction(float x) => FACTOR_LEFT * MathF.Pow(x, -ALPHA - 1.0f) * MathF.Exp(-BETA / x);
    }
}