using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Float;
using NUnit.Framework;

namespace FastRngTests.Float.Distributions
{
    [ExcludeFromCodeCoverage]
    public class CauchyLorentzX0
    {
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestCauchyDistribution01()
        {
            // The properties of the cauchy distribution cannot be tested by mean, media or variance,  
            // cf. https://en.wikipedia.org/wiki/Cauchy_distribution#Explanation_of_undefined_moments
            
            var dist = new FastRng.Float.Distributions.CauchyLorentzX0();
            var fqa = new FrequencyAnalysis();
            var rng = new MultiThreadedRng();
            
            for (var n = 0; n < 100_000; n++)
                fqa.CountThis(await rng.NextNumber(dist));
            
            rng.StopProducer();
            var result = fqa.NormalizeAndPlotEvents(TestContext.WriteLine);
            
            Assert.That(result[0], Is.EqualTo(0.976990739772031f).Within(0.06f));
            Assert.That(result[1], Is.EqualTo(0.948808314586299f).Within(0.06f));
            Assert.That(result[2], Is.EqualTo(0.905284997403441f).Within(0.06f));
            
            Assert.That(result[21], Is.EqualTo(0.168965864241396f).Within(0.04f));
            Assert.That(result[22], Is.EqualTo(0.156877686354491f).Within(0.04f));
            Assert.That(result[23], Is.EqualTo(0.145970509936354f).Within(0.04f));
            
            Assert.That(result[50], Is.EqualTo(0.036533159835978f).Within(0.01f));
            
            Assert.That(result[75], Is.EqualTo(0.016793067514802f).Within(0.01f));
            Assert.That(result[85], Is.EqualTo(0.01316382933791f).Within(0.005f));
            Assert.That(result[90], Is.EqualTo(0.011773781734516f).Within(0.005f));
            
            Assert.That(result[97], Is.EqualTo(0.010168596941156f).Within(0.005f));
            Assert.That(result[98], Is.EqualTo(0.009966272570142f).Within(0.005f));
            Assert.That(result[99], Is.EqualTo(0.00976990739772f).Within(0.005f));
        }

        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestCauchyGeneratorWithRange01()
        {
            var dist = new FastRng.Float.Distributions.CauchyLorentzX0();
            var rng = new MultiThreadedRng();
            var samples = new float[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.NextNumber(-1.0f, 1.0f, dist);
            
            rng.StopProducer();
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(-1.0f), "Min is out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0f), "Max is out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestCauchyGeneratorWithRange02()
        {
            var dist = new FastRng.Float.Distributions.CauchyLorentzX0();
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
        public async Task TestCauchyGeneratorWithRange03()
        {
            var rng = new MultiThreadedRng();
            var dist = new FastRng.Float.Distributions.CauchyLorentzX0 { Random = rng }; // Test default parameters
            
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
            var dist = new FastRng.Float.Distributions.CauchyLorentzX0();
            Assert.DoesNotThrowAsync(async () => await dist.GetDistributedValue());
            Assert.That(await dist.GetDistributedValue(), Is.NaN);
        }
    }
}