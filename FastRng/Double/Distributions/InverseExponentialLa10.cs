using System;
using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double.Distributions
{
    public sealed class InverseExponentialLa10 : Distribution
    {
        private const double LAMBDA = 10.0;
        private const double CONSTANT = 4.539992976248453e-06;

        public InverseExponentialLa10(IRandom rng) : base(rng)
        {
        }
        
        protected override double ShapeFunction(double x) => CONSTANT * LAMBDA * Math.Exp(LAMBDA * x);
    }
}