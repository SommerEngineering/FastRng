using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double
{
    public sealed class ShapeFitter
    {
        private readonly double[] probabilities;
        private readonly double[] samples;
        private readonly IRandom rng;
        private readonly ushort sampleSize;
        private readonly double threshold;
        
        public ShapeFitter(Func<double, double> shapeFunction, IRandom rng, ushort sampleSize = 100, double threshold = 0.99)
        {
            this.rng = rng;
            this.threshold = threshold;
            this.sampleSize = sampleSize;
            this.samples = new double[sampleSize];
            this.probabilities = new double[sampleSize];
            
            var sampleStepSize = 1.0 / sampleSize;
            var nextStep = 0.0 + sampleStepSize;
            for (var n = 0; n < sampleSize; n++)
            {
                this.probabilities[n] = shapeFunction(nextStep);
                nextStep += sampleStepSize;
            }
        }

        public async ValueTask<double> NextNumber(CancellationToken token = default)
        {
            while (!token.IsCancellationRequested)
            {
                var nextNumber = await this.rng.GetUniform(token);
                var nextBucket = (int)Math.Floor(nextNumber * this.sampleSize);
                this.samples[nextBucket] += this.probabilities[nextBucket];

                if (this.samples[nextBucket] >= this.threshold)
                {
                    this.samples[nextBucket] = 0.0;
                    return nextNumber;
                }
            }

            return double.NaN;
        }
    }
}