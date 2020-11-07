using System;
using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double.Distributions
{
    public sealed class WeibullK05La1 : Distribution
    {
        private const double K = 0.5;
        private const double LAMBDA = 1.0;
        private const double CONSTANT = 0.221034183615129;

        public WeibullK05La1(IRandom rng) : base(rng)
        {
        }
        
        protected override double ShapeFunction(double x) => CONSTANT * ( (K / LAMBDA) * Math.Pow(x / LAMBDA, K - 1.0d) * Math.Exp(-Math.Pow(x/LAMBDA, K)));
    }
}