using System;

namespace FastRng.Float.Distributions
{
    public sealed class LaplaceB01M05 : Distribution
    {
        private const float B = 0.1f;
        private const float MU = 0.5f;
        private const float CONSTANT = 0.2f;
        
        private static readonly float FACTOR_LEFT;
        
        static LaplaceB01M05()
        {
            FACTOR_LEFT = CONSTANT / (2.0f * B);
        }

        protected override float ShapeFunction(float x) => FACTOR_LEFT * MathF.Exp(-MathF.Abs(x - MU) / B);
    }
}