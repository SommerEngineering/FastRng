using System;
using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double.Distributions
{
    public sealed class WeibullK05La1 : IDistribution
    {
        private const double K = 0.5;
        private const double LAMBDA = 1.0;
        private const double CONSTANT = 0.221034183615129;

        private ShapeFitter fitter;
        private IRandom random;

        public IRandom Random
        {
            get => this.random;
            set
            {
                this.random = value;
                this.fitter = new ShapeFitter(WeibullK05La1.ShapeFunction, this.random, 100);
            }
        }
        
        private static double ShapeFunction(double x) => CONSTANT * ( (K / LAMBDA) * Math.Pow(x / LAMBDA, K - 1.0d) * Math.Exp(-Math.Pow(x/LAMBDA, K)));

        public async ValueTask<double> GetDistributedValue(CancellationToken token = default)
        {
            if (this.Random == null)
                return double.NaN;
            
            return await this.fitter.NextNumber(token);
        }
    }
}