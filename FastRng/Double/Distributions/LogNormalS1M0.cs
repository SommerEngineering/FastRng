using System;
using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double.Distributions
{
    public sealed class LogNormalS1M0 : IDistribution
    {
        private const double SIGMA = 1.0;
        private const double MU = 0.0;
        private const double CONSTANT = 1.51998658387455;
        
        private static readonly double FACTOR;
        
        private ShapeFitter fitter;
        private IRandom random;

        static LogNormalS1M0()
        {
            FACTOR = SIGMA * Math.Sqrt(2 * Math.PI);
        }
        
        public IRandom Random
        {
            get => this.random;
            set
            {
                this.random = value;
                this.fitter = new ShapeFitter(LogNormalS1M0.ShapeFunction, this.random, 100);
            }
        }

        private static double ShapeFunction(double x) => (CONSTANT / (x * FACTOR)) * Math.Exp( -(Math.Pow(Math.Log(x) - MU, 2) / (2 * Math.Pow(SIGMA, 2))));
        
        public async ValueTask<double> GetDistributedValue(CancellationToken token = default)
        {
            if (this.Random == null)
                return double.NaN;
            
            return await this.fitter.NextNumber(token);
        }
    }
}