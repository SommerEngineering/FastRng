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
        public async Task TestNormalDistribution01()
        {
            const double DESIRED_MEAN = 2.0;
            const double STANDARD_DEVIATION = 5.0;
            
            var dist = new FastRng.Double.Distributions.Normal{ Mean = DESIRED_MEAN, StandardDeviation = STANDARD_DEVIATION };
            var stats = new RunningStatistics();
            var rng = new MultiThreadedRng();
            
            for (var n = 0; n < 100_000; n++)
                stats.Push(await rng.NextNumber(dist));
            
            rng.StopProducer();
            TestContext.WriteLine($"mean={DESIRED_MEAN} vs. {stats.Mean}");
            TestContext.WriteLine($"variance={STANDARD_DEVIATION * STANDARD_DEVIATION} vs {stats.Variance}");
            
            Assert.That(stats.Mean, Is.EqualTo(DESIRED_MEAN).Within(0.4), "Mean is out of range");
            Assert.That(stats.Variance, Is.EqualTo(STANDARD_DEVIATION*STANDARD_DEVIATION).Within(0.4), "Variance is out of range");
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
        public void ParameterTest01()
        {
            var dist = new FastRng.Double.Distributions.Normal();
            
            Assert.DoesNotThrow(() => dist.Mean = -45);
            Assert.DoesNotThrow(() => dist.Mean = 15);
            Assert.DoesNotThrow(() => dist.Mean = 0);

            Assert.Throws<ArgumentOutOfRangeException>(() => dist.StandardDeviation = 0);
            Assert.Throws<ArgumentOutOfRangeException>(() => dist.StandardDeviation = -78);
            Assert.DoesNotThrow(() => dist.StandardDeviation = 0.0001);
            Assert.DoesNotThrow(() => dist.StandardDeviation = 4);
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