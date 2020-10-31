using System;
using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double.Distributions
{
    public sealed class InverseGammaA3B05 : Distribution
    {
        private const double ALPHA = 3.0;
        private const double BETA = 0.5;
        private const double CONSTANT = 0.213922656884911;

        private static readonly double FACTOR_LEFT;
        
        static InverseGammaA3B05()
        {
            var gammaAlpha = MathTools.Gamma(ALPHA);
            var betaToTheAlpha = Math.Pow(BETA, ALPHA);
            
            FACTOR_LEFT = CONSTANT * (betaToTheAlpha / gammaAlpha);
        }

        protected override double ShapeFunction(double x) => FACTOR_LEFT * Math.Pow(x, -ALPHA - 1.0d) * Math.Exp(-BETA / x);
    }
}