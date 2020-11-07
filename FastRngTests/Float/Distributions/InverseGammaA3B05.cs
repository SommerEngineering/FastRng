using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Float;
using NUnit.Framework;

namespace FastRngTests.Float.Distributions
{
    [ExcludeFromCodeCoverage]
    public class InverseGammaA3B05
    {
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestInverseGammaDistribution01()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Float.Distributions.InverseGammaA3B05(rng);
            var fra = new FrequencyAnalysis();
            
            for (var n = 0; n < 100_000; n++)
                fra.CountThis(await dist.NextNumber());
            
            var result = fra.NormalizeAndPlotEvents(TestContext.WriteLine);

            Assert.That(result[0], Is.EqualTo(0.0000000000000003f).Within(0.0000001f));
            Assert.That(result[1], Is.EqualTo(0.0000011605257228f).Within(0.00001f));
            Assert.That(result[2], Is.EqualTo(0.0009536970016103f).Within(0.0015f));
            
            Assert.That(result[21], Is.EqualTo(0.5880485243048120f).Within(0.05f));
            Assert.That(result[22], Is.EqualTo(0.5433842148912880f).Within(0.05f));
            Assert.That(result[23], Is.EqualTo(0.5017780549216030f).Within(0.05f));
            
            Assert.That(result[50], Is.EqualTo(0.0741442015957425f).Within(0.009f));
            
            Assert.That(result[75], Is.EqualTo(0.0207568945092484f).Within(0.006f));
            Assert.That(result[85], Is.EqualTo(0.0136661506653688f).Within(0.006f));
            Assert.That(result[90], Is.EqualTo(0.0112550619601327f).Within(0.006f));
            
            Assert.That(result[97], Is.EqualTo(0.0087026933539773f).Within(0.005f));
            Assert.That(result[98], Is.EqualTo(0.0083995375385004f).Within(0.005f));
            Assert.That(result[99], Is.EqualTo(0.0081094156379928f).Within(0.005f));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestInverseGammaGeneratorWithRange01()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Float.Distributions.InverseGammaA3B05(rng);
            var samples = new float[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await dist.NextNumber(-1.0f, 1.0f);
            
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(-1.0f), "Min out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0f), "Max out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestInverseGammaGeneratorWithRange02()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Float.Distributions.InverseGammaA3B05(rng);
            var samples = new float[1_000];
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
            Assert.Throws<ArgumentNullException>(() => new FastRng.Float.Distributions.InverseGammaA3B05(null));
        }
    }
}