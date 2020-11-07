using System;
using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double.Distributions
{
    public sealed class InverseExponentialLa5 : Distribution
    {
        private const double LAMBDA = 5.0;
        private const double CONSTANT = 0.001347589399817;

        public InverseExponentialLa5(IRandom rng) : base(rng)
        {
        }
        
        protected override double ShapeFunction(double x) => CONSTANT * LAMBDA * Math.Exp(LAMBDA * x);
    }
}