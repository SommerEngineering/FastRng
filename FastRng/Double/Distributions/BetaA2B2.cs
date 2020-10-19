using System;
using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double.Distributions
{
    public sealed class BetaA2B2 : IDistribution
    {
        private const double ALPHA = 2;
        private const double BETA = 2;
        private const double CONSTANT = 4;
        
        private ShapeFitter fitter;
        private IRandom random;

        public BetaA2B2()
        {
        }
        
        public IRandom Random
        {
            get => this.random;
            set
            {
                this.random = value;
                this.fitter = new ShapeFitter(BetaA2B2.ShapeFunction, this.random, 50, 0.99);
            }
        }

        private static double ShapeFunction(double x) => CONSTANT * Math.Pow(x, ALPHA - 1) * Math.Pow(1 - x, BETA - 1);

        public async ValueTask<double> GetDistributedValue(CancellationToken token = default)
        {
            if (this.Random == null)
                return double.NaN;
            
            return await this.fitter.NextNumber(token);
        }
    }
}