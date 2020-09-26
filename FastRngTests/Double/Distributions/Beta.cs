using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Double;
using NUnit.Framework;

namespace FastRngTests.Double.Distributions
{
    [ExcludeFromCodeCoverage]
    public class Beta
    {
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestBetaDistribution01()
        {
            const double A = 7.0;
            const double B = 2.0;
            
            const double MEAN = A / (A + B);
            const double VARIANCE = MEAN * (1 - MEAN) / (A + B + 1);
            
            var dist = new FastRng.Double.Distributions.Beta{ A = A, B = B };
            var stats = new RunningStatistics();
            var rng = new MultiThreadedRng();
            
            for (var n = 0; n < 100_000; n++)
                stats.Push(await rng.NextNumber(dist));
            
            rng.StopProducer();
            TestContext.WriteLine($"mean={MEAN} vs. {stats.Mean}");
            TestContext.WriteLine($"variance={VARIANCE} vs {stats.Variance}");
            
            Assert.That(stats.Mean, Is.EqualTo(MEAN).Within(0.4), "Mean is out of range");
            Assert.That(stats.Variance, Is.EqualTo(VARIANCE).Within(0.4), "Variance is out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestBetaGeneratorWithRange01()
        {
            var rng = new MultiThreadedRng();
            var samples = new double[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.NextNumber(-1.0, 1.0, new FastRng.Double.Distributions.Beta());
            
            rng.StopProducer();
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(-1.0), "Min out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0), "Max out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestBetaGeneratorWithRange02()
        {
            var rng = new MultiThreadedRng();
            var samples = new double[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.NextNumber(0.0, 1.0, new FastRng.Double.Distributions.Beta());
            
            rng.StopProducer();
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(0.0), "Min is out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0), "Max is out of range");
        }

        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void ParameterTest01()
        {
            var dist = new FastRng.Double.Distributions.Beta();
            
            Assert.Throws<ArgumentOutOfRangeException>(() => dist.A = 0);
            Assert.Throws<ArgumentOutOfRangeException>(() => dist.A = -78);
            Assert.DoesNotThrow(() => dist.A = 0.0001);
            Assert.DoesNotThrow(() => dist.A = 4);
            
            Assert.Throws<ArgumentOutOfRangeException>(() => dist.B = 0);
            Assert.Throws<ArgumentOutOfRangeException>(() => dist.B = -78);
            Assert.DoesNotThrow(() => dist.B = 0.0001);
            Assert.DoesNotThrow(() => dist.B = 4);
        }

        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task NoRandomNumberGenerator01()
        {
            var dist = new FastRng.Double.Distributions.Beta();
            Assert.DoesNotThrowAsync(async () => await dist.GetDistributedValue());
            Assert.That(await dist.GetDistributedValue(), Is.NaN);
        }
    }
}