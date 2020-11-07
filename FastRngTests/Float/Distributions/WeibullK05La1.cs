using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Float;
using NUnit.Framework;

namespace FastRngTests.Float.Distributions
{
    [ExcludeFromCodeCoverage]
    public class WeibullK05La1
    {
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestWeibullDistribution01()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Float.Distributions.WeibullK05La1(rng);
            var fra = new FrequencyAnalysis();
            
            for (var n = 0; n < 100_000; n++)
                fra.CountThis(await rng.NextNumber(dist));
            
            var result = fra.NormalizeAndPlotEvents(TestContext.WriteLine);

            Assert.That(result[0], Is.EqualTo(1.000000000f).Within(0.2f));
            Assert.That(result[1], Is.EqualTo(0.678415772f).Within(0.09f));
            Assert.That(result[2], Is.EqualTo(0.536595233f).Within(0.09f));
            
            Assert.That(result[21], Is.EqualTo(0.147406264f).Within(0.02f));
            Assert.That(result[22], Is.EqualTo(0.142654414f).Within(0.02f));
            Assert.That(result[23], Is.EqualTo(0.138217760f).Within(0.02f));
            
            Assert.That(result[50], Is.EqualTo(0.075769787f).Within(0.095f));
            
            Assert.That(result[75], Is.EqualTo(0.053016799f).Within(0.05f));
            Assert.That(result[85], Is.EqualTo(0.047144614f).Within(0.05f));
            Assert.That(result[90], Is.EqualTo(0.044629109f).Within(0.05f));
            
            Assert.That(result[97], Is.EqualTo(0.041484591f).Within(0.05f));
            Assert.That(result[98], Is.EqualTo(0.041067125f).Within(0.05f));
            Assert.That(result[99], Is.EqualTo(0.040656966f).Within(0.05f));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestWeibullGeneratorWithRange01()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Float.Distributions.WeibullK05La1(rng);
            var samples = new float[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.NextNumber(-1.0f, 1.0f, dist);
            
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(-1.0f), "Min out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0f), "Max out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestWeibullGeneratorWithRange02()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Float.Distributions.WeibullK05La1(rng);
            var samples = new float[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.NextNumber(0.0f, 1.0f, dist);
            
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(0.0f), "Min is out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0f), "Max is out of range");
        }

        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void NoRandomNumberGenerator01()
        {
            Assert.Throws<ArgumentNullException>(() => new FastRng.Float.Distributions.WeibullK05La1(null));
        }
    }
}