using System;
using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double.Distributions
{
    public sealed class InverseExponentialLa5 : IDistribution
    {
        private const double LAMBDA = 5.0;
        private const double CONSTANT = 0.001347589399817;

        private ShapeFitter fitter;
        private IRandom random;

        public IRandom Random
        {
            get => this.random;
            set
            {
                this.random = value;
                this.fitter = new ShapeFitter(InverseExponentialLa5.ShapeFunction, this.random, 100);
            }
        }
        
        private static double ShapeFunction(double x) => CONSTANT * LAMBDA * Math.Exp(LAMBDA * x);

        public async ValueTask<double> GetDistributedValue(CancellationToken token = default)
        {
            if (this.Random == null)
                return double.NaN;
            
            return await this.fitter.NextNumber(token);
        }
    }
}