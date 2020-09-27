using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Double;
using NUnit.Framework;

namespace FastRngTests.Double.Distributions
{
    [ExcludeFromCodeCoverage]
    public class CauchyLorentz
    {
        public void TestCauchyDistribution01()
        {
            // The properties of the cauchy distribution cannot be tested by mean, media or variance,  
            // cf. https://en.wikipedia.org/wiki/Cauchy_distribution#Explanation_of_undefined_moments
        }

        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestCauchyGeneratorWithRange01()
        {
            var rng = new MultiThreadedRng();
            var samples = new double[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.NextNumber(-1.0, 1.0, new FastRng.Double.Distributions.CauchyLorentz());
            
            rng.StopProducer();
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(-1.0), "Min is out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0), "Max is out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestCauchyGeneratorWithRange02()
        {
            var rng = new MultiThreadedRng();
            var samples = new double[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.NextNumber(0.0, 1.0, new FastRng.Double.Distributions.CauchyLorentz());
            
            rng.StopProducer();
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(0.0), "Min is out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0), "Max is out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestCauchyGeneratorWithRange03()
        {
            var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.CauchyLorentz { Random = rng }; // Test default parameters
            
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
            var dist = new FastRng.Double.Distributions.CauchyLorentz();
            
            Assert.Throws<ArgumentOutOfRangeException>(() => dist.Scale = 0);
            Assert.Throws<ArgumentOutOfRangeException>(() => dist.Scale = -78);
            Assert.DoesNotThrow(() => dist.Scale = 0.0001);
            Assert.DoesNotThrow(() => dist.Scale = 4);
            
            Assert.DoesNotThrow(() => dist.Median = -45);
            Assert.DoesNotThrow(() => dist.Median = 15);
            Assert.DoesNotThrow(() => dist.Median = 0);
        }

        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task NoRandomNumberGenerator01()
        {
            var dist = new FastRng.Double.Distributions.CauchyLorentz();
            Assert.DoesNotThrowAsync(async () => await dist.GetDistributedValue());
            Assert.That(await dist.GetDistributedValue(), Is.NaN);
        }
    }
}