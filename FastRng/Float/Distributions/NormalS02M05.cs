using System;

namespace FastRng.Float.Distributions
{
    public sealed class NormalS02M05 : Distribution
    {
        private const float SQRT_2_PI = 2.506628275f;
        private const float STDDEV = 0.2f;
        private const float MEAN = 0.5f;

        protected override float ShapeFunction(float x) => 1.0f / (STDDEV * SQRT_2_PI) * MathF.Exp(-0.5f * MathF.Pow((x - MEAN) / STDDEV, 2.0f));
    }
}