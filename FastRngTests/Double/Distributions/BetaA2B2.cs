using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Double;
using NUnit.Framework;

namespace FastRngTests.Double.Distributions
{
    [ExcludeFromCodeCoverage]
    public class BetaA2B2
    {
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestBetaDistribution01()
        {
            var dist = new FastRng.Double.Distributions.BetaA2B2();
            var fqa = new FrequencyAnalysis();
            var rng = new MultiThreadedRng();
            
            for (var n = 0; n < 100_000; n++)
                fqa.CountThis(await rng.NextNumber(dist));
            
            rng.StopProducer();
            var result = fqa.NormalizeAndPlotEvents(TestContext.WriteLine);
            
            Assert.That(result[0], Is.EqualTo(0.0396).Within(0.3));
            Assert.That(result[1], Is.EqualTo(0.0784).Within(0.3));
            Assert.That(result[2], Is.EqualTo(0.1164).Within(0.3));
            
            Assert.That(result[21], Is.EqualTo(0.6864).Within(0.3));
            Assert.That(result[22], Is.EqualTo(0.7084).Within(0.3));
            Assert.That(result[23], Is.EqualTo(0.7296).Within(0.3));
            
            Assert.That(result[50], Is.EqualTo(0.9996).Within(0.3));
            
            Assert.That(result[75], Is.EqualTo(0.7296).Within(0.3));
            Assert.That(result[85], Is.EqualTo(0.4816).Within(0.3));
            Assert.That(result[90], Is.EqualTo(0.3276).Within(0.3));
            
            Assert.That(result[97], Is.EqualTo(0.0784).Within(0.3));
            Assert.That(result[98], Is.EqualTo(0.0396).Within(0.3));
            Assert.That(result[99], Is.EqualTo(0.0000).Within(0.3));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestBetaGeneratorWithRange01()
        {
            var rng = new MultiThreadedRng();
            var samples = new double[1_000];
            var dist = new FastRng.Double.Distributions.BetaA2B2();
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.NextNumber(-1.0, 1.0, dist);

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
            var dist = new FastRng.Double.Distributions.BetaA2B2();
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.NextNumber(0.0, 1.0, dist);
            
            rng.StopProducer();
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(0.0), "Min is out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0), "Max is out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestBetaGeneratorWithRange03()
        {
            var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.BetaA2B2 { Random = rng }; // Test default parameters
            
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
            var dist = new FastRng.Double.Distributions.BetaA2B2();
            Assert.DoesNotThrowAsync(async () => await dist.GetDistributedValue());
            Assert.That(await dist.GetDistributedValue(), Is.NaN);
        }
    }
}