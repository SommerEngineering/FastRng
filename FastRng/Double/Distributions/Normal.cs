using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double.Distributions
{
    public sealed class Normal : IDistribution
    {
        private const double SQRT_2PI = 2.506628275;
        private const double STDDEV = 0.2;
        private const double MEAN = 0.5;
        
        private ShapeFitter fitter;
        private IRandom random;

        public Normal()
        {
        }
        
        public IRandom Random
        {
            get => this.random;
            set
            {
                this.random = value;
                this.fitter = new ShapeFitter(Normal.ShapeFunction, this.random, 50, 1.93);
            }
        }
        
        private static double ShapeFunction(double x) => 1.0 / (STDDEV * SQRT_2PI) * Math.Exp(-0.5 * Math.Pow((x - MEAN) / STDDEV, 2.0));

        public async ValueTask<double> GetDistributedValue(CancellationToken token = default)
        {
            if (this.Random == null)
                return double.NaN;

            return await this.fitter.NextNumber(token);
        }
    }
}