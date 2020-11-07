using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Float;
using NUnit.Framework;

namespace FastRngTests.Float.Distributions
{
    [ExcludeFromCodeCoverage]
    public class BetaA5B2
    {
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestBetaDistribution01()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Float.Distributions.BetaA5B2(rng);
            var fqa = new FrequencyAnalysis();

            for (var n = 0; n < 100_000; n++)
                fqa.CountThis(await dist.NextNumber());
            
            var result = fqa.NormalizeAndPlotEvents(TestContext.WriteLine);
            
            Assert.That(result[0], Is.EqualTo(0.0000001f).Within(0.0000003f));
            Assert.That(result[1], Is.EqualTo(0.0000019f).Within(0.00001f));
            Assert.That(result[2], Is.EqualTo(0.0000096f).Within(0.0004f));

            Assert.That(result[21], Is.EqualTo(0.0222918f).Within(0.03f));
            Assert.That(result[22], Is.EqualTo(0.0262883f).Within(0.03f));
            Assert.That(result[23], Is.EqualTo(0.0307623f).Within(0.03f));
            
            Assert.That(result[50], Is.EqualTo(0.4044237f).Within(0.2f));
            
            Assert.That(result[75], Is.EqualTo(0.9768445f).Within(0.15f));
            Assert.That(result[85], Is.EqualTo(0.9552714f).Within(0.15f));
            Assert.That(result[90], Is.EqualTo(0.8004420f).Within(0.35f));
            
            Assert.That(result[97], Is.EqualTo(0.2250578f).Within(0.03f));
            Assert.That(result[98], Is.EqualTo(0.1171927f).Within(0.03f));
            Assert.That(result[99], Is.EqualTo(0f).Within(0.0004f));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestBetaGeneratorWithRange01()
        {
            using var rng = new MultiThreadedRng();
            var samples = new float[1_000];
            var dist = new FastRng.Float.Distributions.BetaA5B2(rng);
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
            var dist = new FastRng.Float.Distributions.BetaA5B2(rng);
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
            Assert.Throws<ArgumentNullException>(() => new FastRng.Float.Distributions.BetaA5B2(null));
        }
    }
}