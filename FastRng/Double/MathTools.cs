using System;

namespace FastRng.Double
{
    public static class MathTools
    {
        private static readonly double SQRT_2 = Math.Sqrt(2.0);
        private static readonly double SQRT_PI = Math.Sqrt(Math.PI);
        
        public static double Gamma(double z)
        {
            // Source: http://rosettacode.org/wiki/Gamma_function#Go
            
            const double F1 = 6.5;
            const double A1 = .99999999999980993;
            const double A2 = 676.5203681218851;
            const double A3 = 1259.1392167224028;
            const double A4 = 771.32342877765313;
            const double A5 = 176.61502916214059;
            const double A6 = 12.507343278686905;
            const double A7 = .13857109526572012;
            const double A8 = 9.9843695780195716e-6;
            const double A9 = 1.5056327351493116e-7;

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

            return MathTools.SQRT_2 * MathTools.SQRT_PI * Math.Pow(t, z - 0.5) * Math.Exp(-t) * x;
        }
        
        public static double Factorial(double x) => MathTools.Gamma(x + 1.0);

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