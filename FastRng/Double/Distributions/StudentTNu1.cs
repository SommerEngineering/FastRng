using System;
using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double.Distributions
{
    public sealed class StudentTNu1 : Distribution
    {
        private const double NU = 1.0;
        private const double START = 0.0;
        private const double COMPRESS = 1.0;
        private const double CONSTANT = 3.14190548592729;
        
        private static readonly double DIVIDEND;
        private static readonly double DIVISOR;
        private static readonly double EXPONENT;
        
        static StudentTNu1()
        {
            DIVIDEND = MathTools.Gamma((NU + 1.0d) * 0.5d);
            DIVISOR = Math.Sqrt(NU * Math.PI) * MathTools.Gamma(NU * 0.5d);
            EXPONENT = -((NU + 1.0d) * 0.5d);
        }

        protected override double ShapeFunction(double x) => CONSTANT * Math.Pow((DIVIDEND / DIVISOR) * Math.Pow(1.0d + Math.Pow(START + x * COMPRESS, 2) / NU, EXPONENT), COMPRESS);
    }
}