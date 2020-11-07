using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Float;
using NUnit.Framework;

namespace FastRngTests.Float.Distributions
{
    [ExcludeFromCodeCoverage]
    public class BetaA2B2
    {
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestBetaDistribution01()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Float.Distributions.BetaA2B2(rng);
            var fqa = new Float.FrequencyAnalysis();

            for (var n = 0; n < 100_000; n++)
                fqa.CountThis(await dist.NextNumber());
            
            var result = fqa.NormalizeAndPlotEvents(TestContext.WriteLine);
            
            Assert.That(result[0], Is.EqualTo(0.0396f).Within(0.3f));
            Assert.That(result[1], Is.EqualTo(0.0784f).Within(0.3f));
            Assert.That(result[2], Is.EqualTo(0.1164f).Within(0.3f));
            
            Assert.That(result[21], Is.EqualTo(0.6864f).Within(0.3f));
            Assert.That(result[22], Is.EqualTo(0.7084f).Within(0.3f));
            Assert.That(result[23], Is.EqualTo(0.7296f).Within(0.3f));
            
            Assert.That(result[50], Is.EqualTo(0.9996f).Within(0.3f));
            
            Assert.That(result[75], Is.EqualTo(0.7296f).Within(0.3f));
            Assert.That(result[85], Is.EqualTo(0.4816f).Within(0.3f));
            Assert.That(result[90], Is.EqualTo(0.3276f).Within(0.3f));
            
            Assert.That(result[97], Is.EqualTo(0.0784f).Within(0.3f));
            Assert.That(result[98], Is.EqualTo(0.0396f).Within(0.3f));
            Assert.That(result[99], Is.EqualTo(0.0000f).Within(0.3f));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestBetaGeneratorWithRange01()
        {
            using var rng = new MultiThreadedRng();
            var samples = new float[1_000];
            var dist = new FastRng.Float.Distributions.BetaA2B2(rng);
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await dist.NextNumber(-1.0f, 1.0f);

            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(-1.0f), "Min out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0f), "Max out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestBetaGeneratorWithRange02()
        {
            using var rng = new MultiThreadedRng();
            var samples = new float[1_000];
            var dist = new FastRng.Float.Distributions.BetaA2B2(rng);
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await dist.NextNumber(0.0f, 1.0f);
            
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(0.0f), "Min is out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0f), "Max is out of range");
        }

        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void NoRandomNumberGenerator01()
        {
            Assert.Throws<ArgumentNullException>(() => new FastRng.Float.Distributions.BetaA2B2(null));
        }
    }
}