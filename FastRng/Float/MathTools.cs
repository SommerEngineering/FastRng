using System;

namespace FastRng.Float
{
    public static class MathTools
    {
        private static readonly float SQRT_2 = MathF.Sqrt(2.0f);
        private static readonly float SQRT_PI = MathF.Sqrt(MathF.PI);
        
        public static float Gamma(float z)
        {
            // Source: http://rosettacode.org/wiki/Gamma_function#Go
            
            const float F1 = 6.5f;
            const float A1 = .99999999999980993f;
            const float A2 = 676.5203681218851f;
            const float A3 = 1259.1392167224028f;
            const float A4 = 771.32342877765313f;
            const float A5 = 176.61502916214059f;
            const float A6 = 12.507343278686905f;
            const float A7 = .13857109526572012f;
            const float A8 = 9.9843695780195716e-6f;
            const float A9 = 1.5056327351493116e-7f;

            var t = z + F1;
            var x =  A1 +
                     A2 / z -
                     A3 / (z + 1) +
                     A4 / (z + 2) -
                     A5 / (z + 3) +
                     A6 / (z + 4) -
                     A7 / (z + 5) +
                     A8 / (z + 6) +
                     A9 / (z + 7);

            return MathTools.SQRT_2 * MathTools.SQRT_PI * MathF.Pow(t, z - 0.5f) * MathF.Exp(-t) * x;
        }
        
        public static float Factorial(float x) => MathTools.Gamma(x + 1.0f);

        public static ulong Factorial(uint x)
        {
            if (x > 20)
                throw new ArgumentOutOfRangeException(nameof(x), $"Cannot compute {x}!, since ulong.max is 18_446_744_073_709_551_615.");
            
            ulong accumulator = 1;
            for (uint factor = 1; factor <= x; factor++)
                accumulator *= factor;

            return accumulator;
        }

        public static ulong Factorial(int x)
        {
            if(x < 0)
                throw new ArgumentOutOfRangeException(nameof(x), "Given value must be greater as zero.");

            return MathTools.Factorial((uint) x);
        }
    }
}