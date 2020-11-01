using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Float;
using NUnit.Framework;

namespace FastRngTests.Float.Distributions
{
    [ExcludeFromCodeCoverage]
    public class LogNormalS1M0
    {
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestLogNormalDistribution01()
        {
            var dist = new FastRng.Float.Distributions.LogNormalS1M0();
            var fra = new FrequencyAnalysis();
            var rng = new MultiThreadedRng();
            
            for (var n = 0; n < 100_000; n++)
                fra.CountThis(await rng.NextNumber(dist));
            
            rng.StopProducer();
            var result = fra.NormalizeAndPlotEvents(TestContext.WriteLine);

            Assert.That(result[0], Is.EqualTo(0.001505531f).Within(0.003f));
            Assert.That(result[1], Is.EqualTo(0.014408709f).Within(0.01f));
            Assert.That(result[2], Is.EqualTo(0.043222256f).Within(0.02f));
            
            Assert.That(result[21], Is.EqualTo(0.876212056f).Within(0.15f));
            Assert.That(result[22], Is.EqualTo(0.895582226f).Within(0.15f));
            Assert.That(result[23], Is.EqualTo(0.912837250f).Within(0.15f));
            
            Assert.That(result[50], Is.EqualTo(0.948062005f).Within(0.2f));
            
            Assert.That(result[75], Is.EqualTo(0.768584762f).Within(0.089f));
            Assert.That(result[85], Is.EqualTo(0.697303612f).Within(0.089f));
            Assert.That(result[90], Is.EqualTo(0.663570581f).Within(0.089f));
            
            Assert.That(result[97], Is.EqualTo(0.618792767f).Within(0.089f));
            Assert.That(result[98], Is.EqualTo(0.612636410f).Within(0.089f));
            Assert.That(result[99], Is.EqualTo(0.606540679f).Within(0.089f));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestLogNormalGeneratorWithRange01()
        {
            var rng = new MultiThreadedRng();
            var dist = new FastRng.Float.Distributions.LogNormalS1M0();
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
        public async Task TestLogNormalGeneratorWithRange02()
        {
            var rng = new MultiThreadedRng();
            var dist = new FastRng.Float.Distributions.LogNormalS1M0();
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
        public async Task TestLogNormalGeneratorWithRange03()
        {
            var rng = new MultiThreadedRng();
            var dist = new FastRng.Float.Distributions.LogNormalS1M0 { Random = rng }; // Test default parameters
            
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
            var dist = new FastRng.Float.Distributions.LogNormalS1M0();
            Assert.DoesNotThrowAsync(async () => await dist.GetDistributedValue());
            Assert.That(await dist.GetDistributedValue(), Is.NaN);
        }
    }
}