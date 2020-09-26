using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Double;
using NUnit.Framework;

namespace FastRngTests.Double.Distributions
{
    [ExcludeFromCodeCoverage]
    public class LogNormal
    {
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestLogNormalDistribution01()
        {
            const double MU = 0.1;
            const double SIGMA = 0.25;
            var mean = Math.Exp(MU + SIGMA * SIGMA * 0.5);
            var variance = Math.Abs(Math.Exp(SIGMA * SIGMA) - 1) * Math.Exp(2 * MU + SIGMA * SIGMA);
            
            var dist = new FastRng.Double.Distributions.LogNormal{ Mu = MU, Sigma = SIGMA };
            var stats = new RunningStatistics();
            var rng = new MultiThreadedRng();
            
            for (var n = 0; n < 100_000; n++)
                stats.Push(await rng.NextNumber(dist));
            
            rng.StopProducer();
            TestContext.WriteLine($"mean={mean} vs. {stats.Mean}");
            TestContext.WriteLine($"variance={variance} vs {stats.Variance}");
            
            Assert.That(stats.Mean, Is.EqualTo(mean).Within(0.1), "Mean is out of range");
            Assert.That(stats.Variance, Is.EqualTo(variance).Within(0.1), "Variance is out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestLogNormalGeneratorWithRange01()
        {
            var rng = new MultiThreadedRng();
            var samples = new double[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.NextNumber(-1.0, 1.0, new FastRng.Double.Distributions.LogNormal());
            
            rng.StopProducer();
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(-1.0), "Min out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0), "Max out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestLogNormalGeneratorWithRange02()
        {
            var rng = new MultiThreadedRng();
            var samples = new double[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.NextNumber(0.0, 1.0, new FastRng.Double.Distributions.LogNormal());
            
            rng.StopProducer();
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(0.0), "Min is out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0), "Max is out of range");
        }

        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void ParameterTest01()
        {
            var dist = new FastRng.Double.Distributions.LogNormal();
            
            Assert.DoesNotThrow(() => dist.Mu = -45);
            Assert.DoesNotThrow(() => dist.Mu = 15);
            Assert.DoesNotThrow(() => dist.Mu = 0);
            
            Assert.Throws<ArgumentOutOfRangeException>(() => dist.Sigma = 0);
            Assert.Throws<ArgumentOutOfRangeException>(() => dist.Sigma = -78);
            Assert.DoesNotThrow(() => dist.Sigma = 0.0001);
            Assert.DoesNotThrow(() => dist.Sigma = 4);
        }

        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task NoRandomNumberGenerator01()
        {
            var dist = new FastRng.Double.Distributions.LogNormal();
            Assert.DoesNotThrowAsync(async () => await dist.GetDistributedValue());
            Assert.That(await dist.GetDistributedValue(), Is.NaN);
        }
    }
}