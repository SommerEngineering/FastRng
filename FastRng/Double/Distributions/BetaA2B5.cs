using System;
using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double.Distributions
{
    public sealed class BetaA2B5 : IDistribution
    {
        private const double ALPHA = 2;
        private const double BETA = 5;
        private const double CONSTANT = 12.2;
        
        private ShapeFitter fitter;
        private IRandom random;

        public BetaA2B5()
        {
        }
        
        public IRandom Random
        {
            get => this.random;
            set
            {
                this.random = value;
                this.fitter = new ShapeFitter(BetaA2B5.ShapeFunction, this.random, 100);
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