using System;
using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double.Distributions
{
    public sealed class LaplaceB01M0 : IDistribution
    {
        private const double B = 0.1;
        private const double MU = 0.0;
        private const double CONSTANT = 0.221034183615129;
        
        private static readonly double FACTOR_LEFT;
        
        private ShapeFitter fitter;
        private IRandom random;

        static LaplaceB01M0()
        {
            FACTOR_LEFT = CONSTANT / (2.0d * B);
        }
        
        public IRandom Random
        {
            get => this.random;
            set
            {
                this.random = value;
                this.fitter = new ShapeFitter(LaplaceB01M0.ShapeFunction, this.random, 100);
            }
        }

        private static double ShapeFunction(double x) => FACTOR_LEFT * Math.Exp(-Math.Abs(x - MU) / B);

        public async ValueTask<double> GetDistributedValue(CancellationToken token = default)
        {
            if (this.Random == null)
                return double.NaN;
            
            return await this.fitter.NextNumber(token);
        }
    }
}