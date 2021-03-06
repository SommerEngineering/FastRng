using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Float;
using NUnit.Framework;

namespace FastRngTests.Float.Distributions
{
    [ExcludeFromCodeCoverage]
    public class BetaA2B5
    {
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestBetaDistribution01()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Float.Distributions.BetaA2B5(rng);
            var fqa = new Float.FrequencyAnalysis();

            for (var n = 0; n < 100_000; n++)
                fqa.CountThis(await dist.NextNumber());
            
            var result = fqa.NormalizeAndPlotEvents(TestContext.WriteLine);
            
            Assert.That(result[0], Is.EqualTo(0.11719271f).Within(0.3f));
            Assert.That(result[1], Is.EqualTo(0.22505783f).Within(0.3f));
            Assert.That(result[2], Is.EqualTo(0.32401717f).Within(0.3f));
            
            Assert.That(result[21], Is.EqualTo(0.99348410f).Within(0.3f));
            Assert.That(result[22], Is.EqualTo(0.98639433f).Within(0.3f));
            Assert.That(result[23], Is.EqualTo(0.97684451f).Within(0.3f));
            
            Assert.That(result[50], Is.EqualTo(0.35868592f).Within(0.3f));
            
            Assert.That(result[75], Is.EqualTo(0.03076227f).Within(0.03f));
            Assert.That(result[85], Is.EqualTo(0.00403061f).Within(0.03f));
            Assert.That(result[90], Is.EqualTo(0.00109800f).Within(0.01f));
            
            Assert.That(result[97], Is.EqualTo(0.00000191f).Within(0.000003f));
            Assert.That(result[98], Is.EqualTo(0.00000012f).Within(0.0000003f));
            Assert.That(result[99], Is.EqualTo(0.00000000f).Within(0.0000003f));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestBetaGeneratorWithRange01()
        {
            using var rng = new MultiThreadedRng();
            var samples = new float[1_000];
            var dist = new FastRng.Float.Distributions.BetaA2B5(rng);
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
            var dist = new FastRng.Float.Distributions.BetaA2B5(rng);
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
            Assert.Throws<ArgumentNullException>(() => new FastRng.Float.Distributions.BetaA2B5(null));
        }
    }
}