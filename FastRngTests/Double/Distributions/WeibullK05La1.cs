using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Double;
using NUnit.Framework;

namespace FastRngTests.Double.Distributions
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
            var dist = new FastRng.Double.Distributions.WeibullK05La1(rng);
            var fra = new FrequencyAnalysis();
            
            for (var n = 0; n < 100_000; n++)
                fra.CountThis(await dist.NextNumber());
            
            var result = fra.NormalizeAndPlotEvents(TestContext.WriteLine);

            Assert.That(result[0], Is.EqualTo(1.000000000).Within(0.2));
            Assert.That(result[1], Is.EqualTo(0.678415772).Within(0.09));
            Assert.That(result[2], Is.EqualTo(0.536595233).Within(0.09));
            
            Assert.That(result[21], Is.EqualTo(0.147406264).Within(0.02));
            Assert.That(result[22], Is.EqualTo(0.142654414).Within(0.02));
            Assert.That(result[23], Is.EqualTo(0.138217760).Within(0.02));
            
            Assert.That(result[50], Is.EqualTo(0.075769787).Within(0.095));
            
            Assert.That(result[75], Is.EqualTo(0.053016799).Within(0.05));
            Assert.That(result[85], Is.EqualTo(0.047144614).Within(0.05));
            Assert.That(result[90], Is.EqualTo(0.044629109).Within(0.05));
            
            Assert.That(result[97], Is.EqualTo(0.041484591).Within(0.05));
            Assert.That(result[98], Is.EqualTo(0.041067125).Within(0.05));
            Assert.That(result[99], Is.EqualTo(0.040656966).Within(0.05));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestWeibullGeneratorWithRange01()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.WeibullK05La1(rng);
            var samples = new double[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await dist.NextNumber(-1.0, 1.0);
            
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(-1.0), "Min out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0), "Max out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestWeibullGeneratorWithRange02()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.WeibullK05La1(rng);
            var samples = new double[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await dist.NextNumber(0.0, 1.0);
            
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(0.0), "Min is out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0), "Max is out of range");
        }

        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void NoRandomNumberGenerator01()
        {
            Assert.Throws<ArgumentNullException>(() => new FastRng.Double.Distributions.WeibullK05La1(null));
        }
    }
}