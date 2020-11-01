using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Float;
using NUnit.Framework;

namespace FastRngTests.Float.Distributions
{
    [ExcludeFromCodeCoverage]
    public class ChiSquareK4
    {
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestChiSquareDistribution01()
        {
            var dist = new FastRng.Float.Distributions.ChiSquareK4();
            var fqa = new FrequencyAnalysis();
            var rng = new MultiThreadedRng();
            
            for (var n = 0; n < 100_000; n++)
                fqa.CountThis(await rng.NextNumber(dist));

            rng.StopProducer();
            
            var result = fqa.NormalizeAndPlotEvents(TestContext.WriteLine);

            Assert.That(result[0], Is.EqualTo(0.016417705906679f).Within(0.02f));
            Assert.That(result[1], Is.EqualTo(0.032671644513723f).Within(0.02f));
            Assert.That(result[2], Is.EqualTo(0.048763041010352f).Within(0.02f));
            
            Assert.That(result[21], Is.EqualTo(0.32518779111264f).Within(0.05f));
            Assert.That(result[22], Is.EqualTo(0.338273451612642f).Within(0.05f));
            Assert.That(result[23], Is.EqualTo(0.351220492939994f).Within(0.05f));
            
            Assert.That(result[50], Is.EqualTo(0.65209223303425f).Within(0.08f));
            
            Assert.That(result[75], Is.EqualTo(0.857562207152294f).Within(0.099f));
            Assert.That(result[85], Is.EqualTo(0.923072405412387f).Within(0.099f));
            Assert.That(result[90], Is.EqualTo(0.952623623874265f).Within(0.099f));
            
            Assert.That(result[97], Is.EqualTo(0.990616879396201f).Within(0.099f));
            Assert.That(result[98], Is.EqualTo(0.995734077068522f).Within(0.099f));
            Assert.That(result[99], Is.EqualTo(1.00077558852585f).Within(0.1f));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestChiSquareGeneratorWithRange01()
        {
            var dist = new FastRng.Float.Distributions.ChiSquareK4();
            var rng = new MultiThreadedRng();
            var samples = new float[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.NextNumber(-1.0f, 1.0f, dist);
            
            rng.StopProducer();
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(-1.0f), "Min out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0f), "Max out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestChiSquareGeneratorWithRange02()
        {
            var dist = new FastRng.Float.Distributions.ChiSquareK4();
            var rng = new MultiThreadedRng();
            var samples = new float[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.NextNumber(0.0f, 1.0f, dist);
            
            rng.StopProducer();
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(0.0f), "Min is out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0f), "Max is out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestChiSquareGeneratorWithRange03()
        {
            var rng = new MultiThreadedRng();
            var dist = new FastRng.Float.Distributions.ChiSquareK4 { Random = rng }; // Test default parameters
            
            var samples = new float[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await dist.GetDistributedValue();
            
            rng.StopProducer();
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(0.0f), "Min is out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0f), "Max is out of range");
        }

        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task NoRandomNumberGenerator01()
        {
            var dist = new FastRng.Float.Distributions.ChiSquareK4();
            Assert.DoesNotThrowAsync(async () => await dist.GetDistributedValue());
            Assert.That(await dist.GetDistributedValue(), Is.NaN);
        }
    }
}