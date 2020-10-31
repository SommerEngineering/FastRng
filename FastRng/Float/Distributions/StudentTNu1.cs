using System;

namespace FastRng.Float.Distributions
{
    public sealed class StudentTNu1 : Distribution
    {
        private const float NU = 1.0f;
        private const float START = 0.0f;
        private const float COMPRESS = 1.0f;
        private const float CONSTANT = 3.14190548592729f;
        
        private static readonly float DIVIDEND;
        private static readonly float DIVISOR;
        private static readonly float EXPONENT;
        
        static StudentTNu1()
        {
            DIVIDEND = MathTools.Gamma((NU + 1.0f) * 0.5f);
            DIVISOR = MathF.Sqrt(NU * MathF.PI) * MathTools.Gamma(NU * 0.5f);
            EXPONENT = -((NU + 1.0f) * 0.5f);
        }

        protected override float ShapeFunction(float x) => CONSTANT * MathF.Pow((DIVIDEND / DIVISOR) * MathF.Pow(1.0f + MathF.Pow(START + x * COMPRESS, 2f) / NU, EXPONENT), COMPRESS);
    }
}