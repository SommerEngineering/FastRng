using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Double;
using NUnit.Framework;

namespace FastRngTests.Double.Distributions
{
    [ExcludeFromCodeCoverage]
    public class BetaA2B5
    {
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestBetaDistribution01()
        {
            var dist = new FastRng.Double.Distributions.BetaA2B5();
            var fqa = new FrequencyAnalysis();
            var rng = new MultiThreadedRng();
            
            for (var n = 0; n < 100_000; n++)
                fqa.CountThis(await rng.NextNumber(dist));
            
            rng.StopProducer();
            var result = fqa.NormalizeAndPlotEvents(TestContext.WriteLine);
            
            Assert.That(result[0], Is.EqualTo(0.11719271).Within(0.3));
            Assert.That(result[1], Is.EqualTo(0.22505783).Within(0.3));
            Assert.That(result[2], Is.EqualTo(0.32401717).Within(0.3));
            
            Assert.That(result[21], Is.EqualTo(0.99348410).Within(0.3));
            Assert.That(result[22], Is.EqualTo(0.98639433).Within(0.3));
            Assert.That(result[23], Is.EqualTo(0.97684451).Within(0.3));
            
            Assert.That(result[50], Is.EqualTo(0.35868592).Within(0.3));
            
            Assert.That(result[75], Is.EqualTo(0.03076227).Within(0.03));
            Assert.That(result[85], Is.EqualTo(0.00403061).Within(0.03));
            Assert.That(result[90], Is.EqualTo(0.00109800).Within(0.01));
            
            Assert.That(result[97], Is.EqualTo(0.00000191).Within(0.000003));
            Assert.That(result[98], Is.EqualTo(0.00000012).Within(0.0000003));
            Assert.That(result[99], Is.EqualTo(0.00000000).Within(0.0000003));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestBetaGeneratorWithRange01()
        {
            var rng = new MultiThreadedRng();
            var samples = new double[1_000];
            var dist = new FastRng.Double.Distributions.BetaA2B5();
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
            var dist = new FastRng.Double.Distributions.BetaA2B5();
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
            var dist = new FastRng.Double.Distributions.BetaA2B5 { Random = rng }; // Test default parameters
            
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
            var dist = new FastRng.Double.Distributions.BetaA2B5();
            Assert.DoesNotThrowAsync(async () => await dist.GetDistributedValue());
            Assert.That(await dist.GetDistributedValue(), Is.NaN);
        }
    }
}