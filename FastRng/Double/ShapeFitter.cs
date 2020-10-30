using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using FastRng.Double.Distributions;

namespace FastRng.Double
{
    /// <summary>
    /// ShapeFitter is a rejection sampler, cf. https://en.wikipedia.org/wiki/Rejection_sampling
    /// </summary>
    public sealed class ShapeFitter
    {
        private readonly double[] probabilities;
        private readonly IRandom rng;
        private readonly double max;
        private readonly double sampleSize;
        private readonly IDistribution uniform = new Uniform();

        public ShapeFitter(Func<double, double> shapeFunction, IRandom rng, ushort sampleSize = 50)
        {
            this.rng = rng;
            this.sampleSize = sampleSize;
            this.probabilities = new double[sampleSize];

            var sampleStepSize = 1.0d / sampleSize;
            var nextStep = 0.0 + sampleStepSize;
            var maxValue = 0.0d;
            for (var n = 0; n < sampleSize; n++)
            {
                this.probabilities[n] = shapeFunction(nextStep);
                if (this.probabilities[n] > maxValue)
                    maxValue = this.probabilities[n];
                
                nextStep += sampleStepSize;
            }

            this.max = maxValue;
        }

        public async ValueTask<double> NextNumber(CancellationToken token = default)
        {
            while (!token.IsCancellationRequested)
            {
                var x = await this.rng.GetUniform(token);
                var nextBucket = (int)Math.Floor(x * this.sampleSize);
                var threshold = this.probabilities[nextBucket];
                var y = await this.rng.NextNumber(0.0d, this.max, this.uniform, token);
                
                if(y > threshold)
                    continue;

                return x;
            }

            return double.NaN;
        }
    }
}