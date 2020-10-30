using System;
using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double.Distributions
{
    public sealed class CauchyLorentzX1 : IDistribution
    {
        private const double CONSTANT = 0.31;
        private const double SCALE = 0.1;
        private const double MEDIAN = 1.0;
        
        private ShapeFitter fitter;
        private IRandom random;
        
        public IRandom Random
        {
            get => this.random;
            set
            {
                this.random = value;
                this.fitter = new ShapeFitter(CauchyLorentzX1.ShapeFunction, this.random, 100);
            }
        }

        private static double ShapeFunction(double x) => CONSTANT * (1.0 / (Math.PI * SCALE)) * ((SCALE * SCALE) / (Math.Pow(x - MEDIAN, 2) + (SCALE * SCALE)));
        
        public async ValueTask<double> GetDistributedValue(CancellationToken token = default)
        {
            if (this.Random == null)
                return double.NaN;
            
            return await this.fitter.NextNumber(token);
        }
    }
}