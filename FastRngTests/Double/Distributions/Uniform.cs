using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Double;
using NUnit.Framework;

namespace FastRngTests.Double.Distributions
{
    [ExcludeFromCodeCoverage]
    public class Uniform
    {
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestUniformDistribution01()
        {
            const double A = 0.0;
            const double B = 1.0;
            const double MEAN = 0.5 * (A + B);
            const double VARIANCE = (1.0 / 12.0) * (B - A) * (B - A);
            
            var stats = new RunningStatistics();
            var fra = new FrequencyAnalysis();
            using var rng = new MultiThreadedRng();
            
            for (var n = 0; n < 100_000; n++)
            {
                var value = await rng.GetUniform();
                stats.Push(value);
                fra.CountThis(value);
            }

            fra.NormalizeAndPlotEvents(TestContext.WriteLine);
            fra.PlotOccurence(TestContext.WriteLine);
            TestContext.WriteLine($"mean={MEAN} vs. {stats.Mean}");
            TestContext.WriteLine($"variance={VARIANCE} vs {stats.Variance}");
            
            Assert.That(stats.Mean, Is.EqualTo(MEAN).Within(0.01), "Mean is out of range");
            Assert.That(stats.Variance, Is.EqualTo(VARIANCE).Within(0.001), "Variance is out of range");
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
            const double FAILURE_PROBABILITY = 0.001; // probability of test failing with normal distributed input
            const double P_LOW = 0.25 * FAILURE_PROBABILITY;
            const double P_HIGH = 1.0 - 0.25 * FAILURE_PROBABILITY;
            
            var samples = new double[NUM_ROUNDS];
            using var rng = new MultiThreadedRng();
            int n;
             
            for (n = 0; n != NUM_ROUNDS; ++n)
                samples[n] = await rng.GetUniform();
 
            Array.Sort(samples);

            var jMinus = 0;
            var jPlus = 0;
            var kPlus = -double.MaxValue;
            var kMinus = -double.MaxValue;
 
            for (n = 0; n != NUM_ROUNDS; ++n)
            {
                var cdf = samples[n];
                var temp = (n + 1.0) / NUM_ROUNDS - cdf;
                
                if (kPlus < temp)
                {
                    kPlus = temp;
                    jPlus = n;
                }
                
                temp = cdf - (n + 0.0) / NUM_ROUNDS;
                if (kMinus < temp)
                {
                    kMinus = temp;
                    jMinus = n;
                }
            }
 
            var sqrtNumReps = Math.Sqrt(NUM_ROUNDS);
            kPlus *= sqrtNumReps;
            kMinus *= sqrtNumReps;
 
            // We divide the failure probability by four because we have four tests:
            // left and right tests for K+ and K-.
            var cutoffLow = Math.Sqrt(0.5 * Math.Log(1.0 / (1.0 - P_LOW))) - 1.0 / (6.0 * sqrtNumReps);
            var cutoffHigh = Math.Sqrt(0.5 * Math.Log(1.0 / (1.0 - P_HIGH))) - 1.0 / (6.0 * sqrtNumReps);
 
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
            var samples = new double[1_000];
            using var rng = new MultiThreadedRng(); 
            var dist = new FastRng.Double.Distributions.Uniform(rng);
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.NextNumber(-1.0, 1.0, dist);
            
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(-1.0), "Min is out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0), "Max is out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestUniformGeneratorWithRange02()
        {
            var samples = new double[1_000];
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.Uniform(rng);
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.NextNumber(0.0, 1.0, dist);
            
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(0.0), "Min is out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0), "Max is out of range");
        }

        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestUniformGeneratorWithRange04()
        {
            using var rng = new MultiThreadedRng();
            var samples = new double[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.GetUniform();
            
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(0.0), "Min is out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0), "Max is out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestRange05Uint()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.Uniform(rng);
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
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.Uniform(rng);
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
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.Uniform(rng);
            var distribution = new uint[101];
            var runs = 1_000_000;
            for (var n = 0; n < runs; n++)
                distribution[(uint)Math.Floor(await rng.NextNumber(0.0, 100.0, dist))]++;
            
            for (var n = 0; n < distribution.Length - 1; n++)
                Assert.That(distribution[n], Is.GreaterThan(0));
        }
        
        [Test]
        [Category(TestCategories.NORMAL)]
        public async Task TestDistribution001Uint()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.Uniform(rng);
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
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.Uniform(rng);
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
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.Uniform(rng);
            var distribution = new uint[101];
            var runs = 1_000_000;
            for (var n = 0; n < runs; n++)
                distribution[(uint)Math.Floor(await rng.NextNumber(0.0, 100.0, dist))]++;
            
            Assert.That(distribution[..^1].Max() - distribution[..^1].Min(), Is.InRange(0, 600));
        }
        
        [Test]
        [Category(TestCategories.LONG_RUNNING)]
        public async Task TestDistribution002Uint()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.Uniform(rng);
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
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.Uniform(rng);
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
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.Uniform(rng);
            var distribution = new uint[101];
            var runs = 100_000_000;
            for (var n = 0; n < runs; n++)
                distribution[(uint)Math.Floor(await rng.NextNumber(0.0, 100.0, dist))]++;
            
            Assert.That(distribution[..^1].Max() - distribution[..^1].Min(), Is.InRange(0, 6_000));
        }
    }
}