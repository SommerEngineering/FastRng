using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Double;
using NUnit.Framework;

namespace FastRngTests.Double.Distributions
{
    [ExcludeFromCodeCoverage]
    public class Normal
    {
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestNormalDistribution01()
        {
            const double MEAN = 0.5;
            const double STANDARD_DEVIATION = 0.4;
            
            var dist = new FastRng.Double.Distributions.Normal();
            var stats = new RunningStatistics();
            var rng = new MultiThreadedRng();

            for (var n = 0; n < 100_000; n++)
            {
                var nextNumber = await rng.NextNumber(dist);
                stats.Push(nextNumber);
            }

            rng.StopProducer();
            TestContext.WriteLine($"mean={MEAN} vs. {stats.Mean}");
            TestContext.WriteLine($"variance={STANDARD_DEVIATION * STANDARD_DEVIATION} vs {stats.Variance}");
            
            Assert.That(stats.Mean, Is.EqualTo(MEAN).Within(0.4), "Mean is out of range");
            Assert.That(stats.Variance, Is.EqualTo(STANDARD_DEVIATION*STANDARD_DEVIATION).Within(0.1), "Variance is out of range");
        }

        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestNormalGeneratorWithRange01()
        {
            var rng = new MultiThreadedRng();
            var samples = new double[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.NextNumber(-1.0, 1.0, new FastRng.Double.Distributions.Normal());
            
            rng.StopProducer();
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(-1.0), "Min is out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0), "Max is out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestNormalGeneratorWithRange02()
        {
            var rng = new MultiThreadedRng();
            var samples = new double[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.NextNumber(0.0, 1.0, new FastRng.Double.Distributions.Normal());
            
            rng.StopProducer();
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(0.0), "Min is out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0), "Max is out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestNormalGeneratorWithRange03()
        {
            var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.Normal { Random = rng }; // Test default parameters
            
            var samples = new double[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await dist.GetDistributedValue();
            
            rng.StopProducer();
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(0.0), "Min is out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0), "Max is out of range");
        }

        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task NoRandomNumberGenerator01()
        {
            var dist = new FastRng.Double.Distributions.Normal();
            Assert.DoesNotThrowAsync(async () => await dist.GetDistributedValue());
            Assert.That(await dist.GetDistributedValue(), Is.NaN);
        }
    }
}