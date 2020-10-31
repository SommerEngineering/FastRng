using System;
using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double.Distributions
{
    public sealed class ExponentialLa5 : IDistribution
    {
        private const double LAMBDA = 5.0;
        private const double CONSTANT = 0.2103;

        private ShapeFitter fitter;
        private IRandom random;

        public IRandom Random
        {
            get => this.random;
            set
            {
                this.random = value;
                this.fitter = new ShapeFitter(ExponentialLa5.ShapeFunction, this.random, 100);
            }
        }
        
        private static double ShapeFunction(double x) => CONSTANT * LAMBDA * Math.Exp(-LAMBDA * x);

        public async ValueTask<double> GetDistributedValue(CancellationToken token = default)
        {
            if (this.Random == null)
                return double.NaN;
            
            return await this.fitter.NextNumber(token);
        }
    }
}