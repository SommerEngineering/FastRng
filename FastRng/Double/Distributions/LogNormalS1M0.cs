using System;
using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double.Distributions
{
    public sealed class LogNormalS1M0 : Distribution
    {
        private const double SIGMA = 1.0;
        private const double MU = 0.0;
        private const double CONSTANT = 1.51998658387455;
        
        private static readonly double FACTOR;
        
        static LogNormalS1M0()
        {
            FACTOR = SIGMA * Math.Sqrt(2 * Math.PI);
        }

        protected override double ShapeFunction(double x) => (CONSTANT / (x * FACTOR)) * Math.Exp( -(Math.Pow(Math.Log(x) - MU, 2) / (2 * Math.Pow(SIGMA, 2))));
    }
}