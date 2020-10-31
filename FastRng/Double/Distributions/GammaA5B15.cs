using System;
using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double.Distributions
{
    public sealed class GammaA5B15 : IDistribution
    {
        private const double ALPHA = 5.0;
        private const double BETA = 15.0;
        private const double CONSTANT = 0.341344210715475;

        private static readonly double GAMMA_ALPHA;
        private static readonly double BETA_TO_THE_ALPHA;
        
        private ShapeFitter fitter;
        private IRandom random;

        static GammaA5B15()
        {
            GAMMA_ALPHA = MathTools.Gamma(ALPHA);
            BETA_TO_THE_ALPHA = Math.Pow(BETA, ALPHA);
        }
        
        public IRandom Random
        {
            get => this.random;
            set
            {
                this.random = value;
                this.fitter = new ShapeFitter(GammaA5B15.ShapeFunction, this.random, 100);
            }
        }
        
        private static double ShapeFunction(double x) => CONSTANT * ((BETA_TO_THE_ALPHA * Math.Pow(x, ALPHA - 1.0d) * Math.Exp(-BETA * x)) / GAMMA_ALPHA);
        
        public async ValueTask<double> GetDistributedValue(CancellationToken token = default)
        {
            if (this.Random == null)
                return double.NaN;
            
            return await this.fitter.NextNumber(token);
        }
    }
}