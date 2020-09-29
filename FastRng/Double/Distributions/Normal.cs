using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double.Distributions
{
    public sealed class Normal : IDistribution
    {
        private const double SQRT_2PI = 2.506628275;
        private const double STDDEV = 0.4;
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
                this.fitter = new ShapeFitter(ShapeFunction, this.random, 100, 0.99);
            }
        }
        
        private static double ShapeFunction(double x)
        {
            return 1.0 / (STDDEV * SQRT_2PI) * Math.Exp(-Math.Pow((x - MEAN) / STDDEV, 2.0));
        }

        public async ValueTask<double> GetDistributedValue(CancellationToken token = default)
        {
            if (this.Random == null)
                return double.NaN;

            return await this.fitter.NextNumber(); // TODO: Add token!
        }
    }
}