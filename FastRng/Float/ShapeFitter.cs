using System;
using System.Threading;
using System.Threading.Tasks;
using FastRng.Float.Distributions;

namespace FastRng.Float
{
    /// <summary>
    /// ShapeFitter is a rejection sampler, cf. https://en.wikipedia.org/wiki/Rejection_sampling
    /// </summary>
    public sealed class ShapeFitter
    {
        private readonly float[] probabilities;
        private readonly IRandom rng;
        private readonly float max;
        private readonly float sampleSize;
        private readonly IDistribution uniform = new Uniform();

        public ShapeFitter(Func<float, float> shapeFunction, IRandom rng, ushort sampleSize = 50)
        {
            this.rng = rng;
            this.sampleSize = sampleSize;
            this.probabilities = new float[sampleSize];

            var sampleStepSize = 1.0f / sampleSize;
            var nextStep = 0.0f + sampleStepSize;
            var maxValue = 0.0f;
            for (var n = 0; n < sampleSize; n++)
            {
                this.probabilities[n] = shapeFunction(nextStep);
                if (this.probabilities[n] > maxValue)
                    maxValue = this.probabilities[n];
                
                nextStep += sampleStepSize;
            }

            this.max = maxValue;
        }

        public async ValueTask<float> NextNumber(CancellationToken token = default)
        {
            while (!token.IsCancellationRequested)
            {
                var x = await this.rng.GetUniform(token);
                if (float.IsNaN(x))
                    return x;
                
                var nextBucket = (int)MathF.Floor(x * this.sampleSize);
                var threshold = this.probabilities[nextBucket];
                var y = await this.rng.NextNumber(0.0f, this.max, this.uniform, token);
                if (float.IsNaN(y))
                    return y;
                
                if(y > threshold)
                    continue;

                return x;
            }

            return float.NaN;
        }
    }
}