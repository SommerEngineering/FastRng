using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Double;
using NUnit.Framework;

namespace FastRngTests.Double.Distributions
{
    [ExcludeFromCodeCoverage]
    public class StudentT
    {
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestStudentTDistribution01()
        {
            const double DOF = 6;
            const double MEAN = 0.0;
            const double VARIANCE = DOF / (DOF - 2);
            
            var dist = new FastRng.Double.Distributions.StudentT{ DegreesOfFreedom = DOF };
            var stats = new RunningStatistics();
            var rng = new MultiThreadedRng();
            
            for (var n = 0; n < 100_000; n++)
                stats.Push(await rng.NextNumber(dist));
            
            rng.StopProducer();
            TestContext.WriteLine($"mean={MEAN} vs. {stats.Mean}");
            TestContext.WriteLine($"variance={VARIANCE} vs {stats.Variance}");
            
            Assert.That(stats.Mean, Is.EqualTo(MEAN).Within(0.1), "Mean is out of range");
            Assert.That(stats.Variance, Is.EqualTo(VARIANCE).Within(0.1), "Variance is out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestStudentTGeneratorWithRange01()
        {
            var rng = new MultiThreadedRng();
            var samples = new double[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.NextNumber(-1.0, 1.0, new FastRng.Double.Distributions.StudentT());
            
            rng.StopProducer();
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(-1.0), "Min out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0), "Max out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestStudentTGeneratorWithRange02()
        {
            var rng = new MultiThreadedRng();
            var samples = new double[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.NextNumber(0.0, 1.0, new FastRng.Double.Distributions.StudentT());
            
            rng.StopProducer();
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(0.0), "Min is out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0), "Max is out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestStudentTGeneratorWithRange03()
        {
            var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.StudentT { Random = rng }; // Test default parameters
            
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
            var dist = new FastRng.Double.Distributions.StudentT();
            
            Assert.Throws<ArgumentOutOfRangeException>(() => dist.DegreesOfFreedom = 0);
            Assert.Throws<ArgumentOutOfRangeException>(() => dist.DegreesOfFreedom = -78);
            Assert.DoesNotThrow(() => dist.DegreesOfFreedom = 0.0001);
            Assert.DoesNotThrow(() => dist.DegreesOfFreedom = 4);
        }

        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task NoRandomNumberGenerator01()
        {
            var dist = new FastRng.Double.Distributions.StudentT();
            Assert.DoesNotThrowAsync(async () => await dist.GetDistributedValue());
            Assert.That(await dist.GetDistributedValue(), Is.NaN);
        }
    }
}