using System;

namespace FastRng.Float.Distributions
{
    public sealed class LogNormalS1M0 : Distribution
    {
        private const float SIGMA = 1.0f;
        private const float MU = 0.0f;
        private const float CONSTANT = 1.51998658387455f;
        
        private static readonly float FACTOR;
        
        static LogNormalS1M0()
        {
            FACTOR = SIGMA * MathF.Sqrt(2f * MathF.PI);
        }

        protected override float ShapeFunction(float x) => (CONSTANT / (x * FACTOR)) * MathF.Exp( -(MathF.Pow(MathF.Log(x) - MU, 2f) / (2f * MathF.Pow(SIGMA, 2f))));
    }
}