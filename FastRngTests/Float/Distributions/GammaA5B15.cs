using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Float;
using NUnit.Framework;

namespace FastRngTests.Float.Distributions
{
    [ExcludeFromCodeCoverage]
    public class GammaA5B15
    {
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestGammaDistribution01()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Float.Distributions.GammaA5B15(rng);
            var fra = new FrequencyAnalysis();
            
            for (var n = 0; n < 100_000; n++)
                fra.CountThis(await rng.NextNumber(dist));
            
            var result = fra.NormalizeAndPlotEvents(TestContext.WriteLine);

            Assert.That(result[0], Is.EqualTo(0.0000929594237282f).Within(0.0008f));
            Assert.That(result[1], Is.EqualTo(0.0012801746797876f).Within(0.002f));
            Assert.That(result[2], Is.EqualTo(0.0055781488254349f).Within(0.004f));
            
            Assert.That(result[21], Is.EqualTo(0.9331608887752720f).Within(0.09f));
            Assert.That(result[22], Is.EqualTo(0.9594734828891280f).Within(0.09f));
            Assert.That(result[23], Is.EqualTo(0.9790895765535350f).Within(0.09f));
            
            Assert.That(result[50], Is.EqualTo(0.3478287795336570f).Within(0.06f));
            
            Assert.That(result[75], Is.EqualTo(0.0403399049422936f).Within(0.009f));
            Assert.That(result[85], Is.EqualTo(0.0163628388658126f).Within(0.009f));
            Assert.That(result[90], Is.EqualTo(0.0097147611446660f).Within(0.005f));
            
            Assert.That(result[97], Is.EqualTo(0.0041135143233153f).Within(0.008f));
            Assert.That(result[98], Is.EqualTo(0.0036872732029996f).Within(0.008f));
            Assert.That(result[99], Is.EqualTo(0.0033038503429554f).Within(0.008f));
        }

        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestGammaGeneratorWithRange01()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Float.Distributions.GammaA5B15(rng);
            var samples = new float[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.NextNumber(-1.0f, 1.0f, dist);
            
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(-1.0f), "Min is out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0f), "Max is out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestGammaGeneratorWithRange02()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Float.Distributions.GammaA5B15(rng);
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
            Assert.Throws<ArgumentNullException>(() => new FastRng.Float.Distributions.GammaA5B15(null));
        }
    }
}