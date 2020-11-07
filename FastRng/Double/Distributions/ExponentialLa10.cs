using System;
using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double.Distributions
{
    public sealed class ExponentialLa10 : Distribution
    {
        private const double LAMBDA = 10.0;
        private const double CONSTANT = 0.1106;

        public ExponentialLa10(IRandom rng) : base(rng)
        {
        }
        
        protected override double ShapeFunction(double x) => CONSTANT * LAMBDA * Math.Exp(-LAMBDA * x);
    }
}