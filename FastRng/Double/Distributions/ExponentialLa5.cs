using System;
using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double.Distributions
{
    public sealed class ExponentialLa5 : Distribution
    {
        private const double LAMBDA = 5.0;
        private const double CONSTANT = 0.2103;

        public ExponentialLa5(IRandom rng) : base(rng)
        {
        }
        
        protected override double ShapeFunction(double x) => CONSTANT * LAMBDA * Math.Exp(-LAMBDA * x);
    }
}