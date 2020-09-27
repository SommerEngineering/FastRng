using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Double;
using NUnit.Framework;

namespace FastRngTests.Double.Distributions
{
    [ExcludeFromCodeCoverage]
    public class Weibull
    {
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestWeibullDistribution01()
        {
            const double SHAPE = 2;
            const double SCALE = 3;
            const double VARIANCE = 9 * (1 - Math.PI / 4);
            var mean = 3 * Math.Sqrt(Math.PI) / 2;

            var dist = new FastRng.Double.Distributions.Weibull{ Shape = SHAPE, Scale = SCALE };
            var stats = new RunningStatistics();
            var rng = new MultiThreadedRng();
            
            for (var n = 0; n < 100_000; n++)
                stats.Push(await rng.NextNumber(dist));
            
            rng.StopProducer();
            TestContext.WriteLine($"mean={mean} vs. {stats.Mean}");
            TestContext.WriteLine($"variance={VARIANCE} vs {stats.Variance}");
            
            Assert.That(stats.Mean, Is.EqualTo(mean).Within(0.2), "Mean is out of range");
            Assert.That(stats.Variance, Is.EqualTo(VARIANCE).Within(0.2), "Variance is out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestWeibullGeneratorWithRange01()
        {
            var rng = new MultiThreadedRng();
            var samples = new double[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.NextNumber(-1.0, 1.0, new FastRng.Double.Distributions.Weibull());
            
            rng.StopProducer();
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(-1.0), "Min out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0), "Max out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestWeibullGeneratorWithRange02()
        {
            var rng = new MultiThreadedRng();
            var samples = new double[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.NextNumber(0.0, 1.0, new FastRng.Double.Distributions.Weibull());
            
            rng.StopProducer();
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(0.0), "Min is out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0), "Max is out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestWeibullGeneratorWithRange03()
        {
            var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.Weibull { Random = rng }; // Test default parameters
            
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
        public void ParameterTest01()
        {
            var dist = new FastRng.Double.Distributions.Weibull();
            
            Assert.Throws<ArgumentOutOfRangeException>(() => dist.Scale = 0);
            Assert.Throws<ArgumentOutOfRangeException>(() => dist.Scale = -78);
            Assert.DoesNotThrow(() => dist.Scale = 0.0001);
            Assert.DoesNotThrow(() => dist.Scale = 4);
            
            Assert.Throws<ArgumentOutOfRangeException>(() => dist.Shape = 0);
            Assert.Throws<ArgumentOutOfRangeException>(() => dist.Shape = -78);
            Assert.DoesNotThrow(() => dist.Shape = 0.0001);
            Assert.DoesNotThrow(() => dist.Shape = 4);
        }

        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task NoRandomNumberGenerator01()
        {
            var dist = new FastRng.Double.Distributions.Weibull();
            Assert.DoesNotThrowAsync(async () => await dist.GetDistributedValue());
            Assert.That(await dist.GetDistributedValue(), Is.NaN);
        }
    }
}