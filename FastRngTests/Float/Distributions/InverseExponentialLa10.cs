using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Float;
using NUnit.Framework;

namespace FastRngTests.Float.Distributions
{
    [ExcludeFromCodeCoverage]
    public class InverseExponentialLa10
    {
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestExponentialDistribution01()
        {
            var dist = new FastRng.Float.Distributions.InverseExponentialLa10();
            var fqa = new FrequencyAnalysis();
            var rng = new MultiThreadedRng();
            
            for (var n = 0; n < 100_000; n++)
                fqa.CountThis(await rng.NextNumber(dist));
            
            rng.StopProducer();
            var result = fqa.NormalizeAndPlotEvents(TestContext.WriteLine);

            Assert.That(result[0], Is.EqualTo(0.0000501746820562f).Within(0.0003f));
            Assert.That(result[1], Is.EqualTo(0.0000554515994322f).Within(0.0003f));
            Assert.That(result[2], Is.EqualTo(0.0000612834950532f).Within(0.0003f));
            
            Assert.That(result[21], Is.EqualTo(0.00040973497898f).Within(0.00045f));
            Assert.That(result[22], Is.EqualTo(0.000452827182887f).Within(0.00050f));
            Assert.That(result[23], Is.EqualTo(0.000500451433441f).Within(0.0006f));
            
            Assert.That(result[50], Is.EqualTo(0.007446583070924f).Within(0.003f));
            
            Assert.That(result[75], Is.EqualTo(0.090717953289412f).Within(0.02f));
            Assert.That(result[85], Is.EqualTo(0.246596963941606f).Within(0.05f));
            Assert.That(result[90], Is.EqualTo(0.406569659740598f).Within(0.08f));
            
            Assert.That(result[97], Is.EqualTo(0.81873075307798f).Within(0.08f));
            Assert.That(result[98], Is.EqualTo(0.904837418035957f).Within(0.08f));
            Assert.That(result[99], Is.EqualTo(0.999999999999999f).Within(0.08f));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestExponentialGeneratorWithRange01()
        {
            var dist = new FastRng.Float.Distributions.InverseExponentialLa10();
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
        public async Task TestExponentialGeneratorWithRange02()
        {
            var dist = new FastRng.Float.Distributions.InverseExponentialLa10();
            var rng = new MultiThreadedRng();
            var samples = new float[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.NextNumber(0.0f, 1.0f, dist);
            
            rng.StopProducer();
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(0.0f), "Min is out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0f), "Max is out of range");
        }

        [Test] [Category(TestCategories.COVER)] [Category(TestCategories.NORMAL)]
        public async Task TestExponentialGeneratorWithRange03()
        {
            var rng = new MultiThreadedRng();
            var dist = new FastRng.Float.Distributions.InverseExponentialLa10 { Random = rng }; // Test default parameters
            
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
            var dist = new FastRng.Float.Distributions.InverseExponentialLa10();
            Assert.DoesNotThrowAsync(async () => await dist.GetDistributedValue());
            Assert.That(await dist.GetDistributedValue(), Is.NaN);
        }
    }
}