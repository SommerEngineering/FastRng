using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Double;
using NUnit.Framework;

namespace FastRngTests.Double.Distributions
{
    [ExcludeFromCodeCoverage]
    public class NormalS02M05
    {
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestNormalDistribution01()
        {
            const double MEAN = 0.5;
            const double STANDARD_DEVIATION = 0.2;
            
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.NormalS02M05(rng);
            var stats = new RunningStatistics();
            var fra = new FrequencyAnalysis();

            for (var n = 0; n < 100_000; n++)
            {
                var nextNumber = await rng.NextNumber(dist);
                stats.Push(nextNumber);
                fra.CountThis(nextNumber);
            }

            fra.NormalizeAndPlotEvents(TestContext.WriteLine);
            
            TestContext.WriteLine($"mean={MEAN} vs. {stats.Mean}");
            TestContext.WriteLine($"variance={STANDARD_DEVIATION * STANDARD_DEVIATION} vs {stats.Variance}");
            
            Assert.That(stats.Mean, Is.EqualTo(MEAN).Within(0.01), "Mean is out of range");
            Assert.That(stats.Variance, Is.EqualTo(STANDARD_DEVIATION*STANDARD_DEVIATION).Within(0.01), "Variance is out of range");
        }

        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestNormalGeneratorWithRange01()
        {
            using var rng = new MultiThreadedRng();
            var samples = new double[1_000];
            var dist = new FastRng.Double.Distributions.NormalS02M05(rng);
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.NextNumber(-1.0, 1.0, dist);
            
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(-1.0), "Min is out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0), "Max is out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestNormalGeneratorWithRange02()
        {
            using var rng = new MultiThreadedRng();
            var samples = new double[1_000];
            var dist = new FastRng.Double.Distributions.NormalS02M05(rng);
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.NextNumber(0.0, 1.0, dist);
            
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(0.0), "Min is out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0), "Max is out of range");
        }

        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void NoRandomNumberGenerator01()
        {
            Assert.Throws<ArgumentNullException>(() => new FastRng.Double.Distributions.NormalS02M05(null));
        }
    }
}