using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Float;
using NUnit.Framework;

namespace FastRngTests.Float.Distributions
{
    [ExcludeFromCodeCoverage]
    public class NormalS02M05
    {
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestNormalDistribution01()
        {
            const float MEAN = 0.5f;
            const float STANDARD_DEVIATION = 0.2f;
            
            var dist = new FastRng.Float.Distributions.NormalS02M05();
            var stats = new RunningStatistics();
            var fra = new FrequencyAnalysis();
            var rng = new MultiThreadedRng();

            for (var n = 0; n < 100_000; n++)
            {
                var nextNumber = await rng.NextNumber(dist);
                stats.Push(nextNumber);
                fra.CountThis(nextNumber);
            }

            rng.StopProducer();
            fra.NormalizeAndPlotEvents(TestContext.WriteLine);
            
            TestContext.WriteLine($"mean={MEAN} vs. {stats.Mean}");
            TestContext.WriteLine($"variance={STANDARD_DEVIATION * STANDARD_DEVIATION} vs {stats.Variance}");
            
            Assert.That(stats.Mean, Is.EqualTo(MEAN).Within(0.01f), "Mean is out of range");
            Assert.That(stats.Variance, Is.EqualTo(STANDARD_DEVIATION*STANDARD_DEVIATION).Within(0.01f), "Variance is out of range");
        }

        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestNormalGeneratorWithRange01()
        {
            var rng = new MultiThreadedRng();
            var samples = new float[1_000];
            var dist = new FastRng.Float.Distributions.NormalS02M05();
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.NextNumber(-1.0f, 1.0f, dist);
            
            rng.StopProducer();
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(-1.0f), "Min is out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0f), "Max is out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestNormalGeneratorWithRange02()
        {
            var rng = new MultiThreadedRng();
            var samples = new float[1_000];
            var dist = new FastRng.Float.Distributions.NormalS02M05();
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.NextNumber(0.0f, 1.0f, dist);
            
            rng.StopProducer();
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(0.0f), "Min is out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0f), "Max is out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestNormalGeneratorWithRange03()
        {
            var rng = new MultiThreadedRng();
            var dist = new FastRng.Float.Distributions.NormalS02M05 { Random = rng }; // Test default parameters
            
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
        public async Task NoRandomNumberGenerator01()
        {
            var dist = new FastRng.Float.Distributions.NormalS02M05();
            Assert.DoesNotThrowAsync(async () => await dist.GetDistributedValue());
            Assert.That(await dist.GetDistributedValue(), Is.NaN);
        }
    }
}