using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Float;
using NUnit.Framework;

namespace FastRngTests.Float.Distributions
{
    [ExcludeFromCodeCoverage]
    public class Uniform
    {
        private readonly IRandom rng = new MultiThreadedRng();
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestUniformDistribution01()
        {
            const float A = 0.0f;
            const float B = 1.0f;
            const float MEAN = 0.5f * (A + B);
            const float VARIANCE = (1.0f / 12.0f) * (B - A) * (B - A);
            
            var stats = new RunningStatistics();
            var fra = new FrequencyAnalysis();
            var rng = new MultiThreadedRng();
            
            for (var n = 0; n < 100_000; n++)
            {
                var value = await rng.GetUniform();
                stats.Push(value);
                fra.CountThis(value);
            }

            rng.StopProducer();
            fra.NormalizeAndPlotEvents(TestContext.WriteLine);
            fra.PlotOccurence(TestContext.WriteLine);
            TestContext.WriteLine($"mean={MEAN} vs. {stats.Mean}");
            TestContext.WriteLine($"variance={VARIANCE} vs {stats.Variance}");
            
            Assert.That(stats.Mean, Is.EqualTo(MEAN).Within(0.01f), "Mean is out of range");
            Assert.That(stats.Variance, Is.EqualTo(VARIANCE).Within(0.001f), "Variance is out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task KolmogorovSmirnovTest()
        {
            // Kolmogorov-Smirnov test for distributions.
            // See Knuth volume 2, page 48-51 (third edition).
            // This test should *fail* on average one time in 1000 runs.
            // That's life with random number generators: if the test passed all the time, 
            // the source wouldn't be random enough!  If the test were to fail more frequently,
            // the most likely explanation would be a bug in the code.
            
            const int NUM_ROUNDS = 10_000;
            const float FAILURE_PROBABILITY = 0.001f; // probability of test failing with normal distributed input
            const float P_LOW = 0.25f * FAILURE_PROBABILITY;
            const float P_HIGH = 1.0f - 0.25f * FAILURE_PROBABILITY;
            
            var samples = new float[NUM_ROUNDS];
            var rng = new MultiThreadedRng();
            int n;
             
            for (n = 0; n != NUM_ROUNDS; ++n)
                samples[n] = await rng.GetUniform();
 
            rng.StopProducer();
            Array.Sort(samples);

            var jMinus = 0;
            var jPlus = 0;
            var kPlus = -float.MaxValue;
            var kMinus = -float.MaxValue;
 
            for (n = 0; n != NUM_ROUNDS; ++n)
            {
                var cdf = samples[n];
                var temp = (n + 1.0f) / NUM_ROUNDS - cdf;
                
                if (kPlus < temp)
                {
                    kPlus = temp;
                    jPlus = n;
                }
                
                temp = cdf - (n + 0.0f) / NUM_ROUNDS;
                if (kMinus < temp)
                {
                    kMinus = temp;
                    jMinus = n;
                }
            }
 
            var sqrtNumReps = MathF.Sqrt(NUM_ROUNDS);
            kPlus *= sqrtNumReps;
            kMinus *= sqrtNumReps;
 
            // We divide the failure probability by four because we have four tests:
            // left and right tests for K+ and K-.
            var cutoffLow = MathF.Sqrt(0.5f * MathF.Log(1.0f / (1.0f - P_LOW))) - 1.0f / (6.0f * sqrtNumReps);
            var cutoffHigh = MathF.Sqrt(0.5f * MathF.Log(1.0f / (1.0f - P_HIGH))) - 1.0f / (6.0f * sqrtNumReps);
 
            TestContext.WriteLine($"K+ = {kPlus} | K- = {kMinus}");
            TestContext.WriteLine($"K+ max at position {jPlus} = {samples[jPlus]}");
            TestContext.WriteLine($"K- max at position {jMinus} = {samples[jMinus]}");
            TestContext.WriteLine($"Acceptable interval: [{cutoffLow}, {cutoffHigh}]");

            Assert.That(kPlus, Is.GreaterThanOrEqualTo(cutoffLow), "K+ is lower than low cutoff");
            Assert.That(kPlus, Is.LessThanOrEqualTo(cutoffHigh), "K+ is higher than high cutoff");
            Assert.That(kMinus, Is.GreaterThanOrEqualTo(cutoffLow), "K- is lower than low cutoff");
            Assert.That(kMinus, Is.LessThanOrEqualTo(cutoffHigh), "K- is lower than high cutoff");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestUniformGeneratorWithRange01()
        {
            var samples = new float[1_000];
            var dist = new FastRng.Float.Distributions.Uniform();
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.NextNumber(-1.0f, 1.0f, dist);
            
            rng.StopProducer();
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(-1.0f), "Min is out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0f), "Max is out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestUniformGeneratorWithRange02()
        {
            var samples = new float[1_000];
            var dist = new FastRng.Float.Distributions.Uniform();
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.NextNumber(0.0f, 1.0f, dist);
            
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(0.0f), "Min is out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0f), "Max is out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestUniformGeneratorWithRange03()
        {
            var rng = new MultiThreadedRng();
            var dist = new FastRng.Float.Distributions.Uniform { Random = rng }; // Test default parameters
            
            var samples = new float[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await dist.GetDistributedValue();
            
            rng.StopProducer();
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(0.0f), "Min is out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0f), "Max is out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestUniformGeneratorWithRange04()
        {
            var rng = new MultiThreadedRng();
            var samples = new float[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.GetUniform();
            
            rng.StopProducer();
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(0.0f), "Min is out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0f), "Max is out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestRange05Uint()
        {
            var dist = new FastRng.Float.Distributions.Uniform();
            var distribution = new uint[101];
            var runs = 1_000_000;
            for (var n = 0; n < runs; n++)
                distribution[await rng.NextNumber(0, 100, dist)]++;
            
            for (var n = 0; n < distribution.Length - 1; n++)
                Assert.That(distribution[n], Is.GreaterThan(0));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestRange05Ulong()
        {
            var dist = new FastRng.Float.Distributions.Uniform();
            var distribution = new uint[101];
            var runs = 1_000_000;
            for (var n = 0; n < runs; n++)
                distribution[await rng.NextNumber(0UL, 100, dist)]++;
            
            for (var n = 0; n < distribution.Length - 1; n++)
                Assert.That(distribution[n], Is.GreaterThan(0));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestRange05Float()
        {
            var dist = new FastRng.Float.Distributions.Uniform();
            var distribution = new uint[101];
            var runs = 1_000_000;
            for (var n = 0; n < runs; n++)
                distribution[(uint)MathF.Floor(await rng.NextNumber(0.0f, 100.0f, dist))]++;
            
            for (var n = 0; n < distribution.Length - 1; n++)
                Assert.That(distribution[n], Is.GreaterThan(0));
        }
        
        [Test]
        [Category(TestCategories.NORMAL)]
        public async Task TestDistribution001Uint()
        {
            var dist = new FastRng.Float.Distributions.Uniform();
            var distribution = new uint[101];
            var runs = 1_000_000;
            for (var n = 0; n < runs; n++)
                distribution[await rng.NextNumber(0, 100, dist)]++;
            
            Assert.That(distribution[..^1].Max() - distribution[..^1].Min(), Is.InRange(0, 600));
        }
        
        [Test]
        [Category(TestCategories.NORMAL)]
        public async Task TestDistribution001Ulong()
        {
            var dist = new FastRng.Float.Distributions.Uniform();
            var distribution = new uint[101];
            var runs = 1_000_000;
            for (var n = 0; n < runs; n++)
                distribution[await rng.NextNumber(0UL, 100, dist)]++;
            
            Assert.That(distribution[..^1].Max() - distribution[..^1].Min(), Is.InRange(0, 600));
        }
        
        [Test]
        [Category(TestCategories.NORMAL)]
        public async Task TestDistribution001Float()
        {
            var dist = new FastRng.Float.Distributions.Uniform();
            var distribution = new uint[101];
            var runs = 1_000_000;
            for (var n = 0; n < runs; n++)
                distribution[(uint)MathF.Floor(await rng.NextNumber(0.0f, 100.0f, dist))]++;
            
            Assert.That(distribution[..^1].Max() - distribution[..^1].Min(), Is.InRange(0, 600));
        }
        
        [Test]
        [Category(TestCategories.LONG_RUNNING)]
        public async Task TestDistribution002Uint()
        {
            var dist = new FastRng.Float.Distributions.Uniform();
            var distribution = new uint[101];
            var runs = 100_000_000;
            for (var n = 0; n < runs; n++)
                distribution[await rng.NextNumber(0, 100, dist)]++;
            
            Assert.That(distribution[..^1].Max() - distribution[..^1].Min(), Is.InRange(0, 6_000));
        }
        
        [Test]
        [Category(TestCategories.LONG_RUNNING)]
        public async Task TestDistribution002Ulong()
        {
            var dist = new FastRng.Float.Distributions.Uniform();
            var distribution = new uint[101];
            var runs = 100_000_000;
            for (var n = 0; n < runs; n++)
                distribution[await rng.NextNumber(0UL, 100, dist)]++;
            
            Assert.That(distribution[..^1].Max() - distribution[..^1].Min(), Is.InRange(0, 6_000));
        }
        
        [Test]
        [Category(TestCategories.LONG_RUNNING)]
        public async Task TestDistribution002Float()
        {
            var dist = new FastRng.Float.Distributions.Uniform();
            var distribution = new uint[101];
            var runs = 100_000_000;
            for (var n = 0; n < runs; n++)
                distribution[(uint)MathF.Floor(await rng.NextNumber(0.0f, 100.0f, dist))]++;
            
            Assert.That(distribution[..^1].Max() - distribution[..^1].Min(), Is.InRange(0, 6_000));
        }
    }
}