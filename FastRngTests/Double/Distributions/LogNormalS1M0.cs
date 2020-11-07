using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Double;
using NUnit.Framework;

namespace FastRngTests.Double.Distributions
{
    [ExcludeFromCodeCoverage]
    public class LogNormalS1M0
    {
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestLogNormalDistribution01()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.LogNormalS1M0(rng);
            var fra = new FrequencyAnalysis();
            
            for (var n = 0; n < 100_000; n++)
                fra.CountThis(await rng.NextNumber(dist));
            
            var result = fra.NormalizeAndPlotEvents(TestContext.WriteLine);

            Assert.That(result[0], Is.EqualTo(0.001505531).Within(0.003));
            Assert.That(result[1], Is.EqualTo(0.014408709).Within(0.01));
            Assert.That(result[2], Is.EqualTo(0.043222256).Within(0.02));
            
            Assert.That(result[21], Is.EqualTo(0.876212056).Within(0.15));
            Assert.That(result[22], Is.EqualTo(0.895582226).Within(0.15));
            Assert.That(result[23], Is.EqualTo(0.912837250).Within(0.15));
            
            Assert.That(result[50], Is.EqualTo(0.948062005).Within(0.2));
            
            Assert.That(result[75], Is.EqualTo(0.768584762).Within(0.089));
            Assert.That(result[85], Is.EqualTo(0.697303612).Within(0.089));
            Assert.That(result[90], Is.EqualTo(0.663570581).Within(0.089));
            
            Assert.That(result[97], Is.EqualTo(0.618792767).Within(0.089));
            Assert.That(result[98], Is.EqualTo(0.612636410).Within(0.089));
            Assert.That(result[99], Is.EqualTo(0.606540679).Within(0.089));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestLogNormalGeneratorWithRange01()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.LogNormalS1M0(rng);
            var samples = new double[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.NextNumber(-1.0, 1.0, dist);
            
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(-1.0), "Min out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0), "Max out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestLogNormalGeneratorWithRange02()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.LogNormalS1M0(rng);
            var samples = new double[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.NextNumber(0.0, 1.0, dist);
            
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(0.0), "Min is out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0), "Max is out of range");
        }

        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void NoRandomNumberGenerator01()
        {
            Assert.Throws<ArgumentNullException>(() => new FastRng.Double.Distributions.LogNormalS1M0(null));
        }
    }
}