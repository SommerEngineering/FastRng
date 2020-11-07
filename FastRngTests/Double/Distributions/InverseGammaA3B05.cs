using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Double;
using NUnit.Framework;

namespace FastRngTests.Double.Distributions
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
            var dist = new FastRng.Double.Distributions.InverseGammaA3B05(rng);
            var fra = new FrequencyAnalysis();
            
            for (var n = 0; n < 100_000; n++)
                fra.CountThis(await rng.NextNumber(dist));
            
            var result = fra.NormalizeAndPlotEvents(TestContext.WriteLine);

            Assert.That(result[0], Is.EqualTo(0.0000000000000003).Within(0.0000001));
            Assert.That(result[1], Is.EqualTo(0.0000011605257228).Within(0.00001));
            Assert.That(result[2], Is.EqualTo(0.0009536970016103).Within(0.0015));
            
            Assert.That(result[21], Is.EqualTo(0.5880485243048120).Within(0.05));
            Assert.That(result[22], Is.EqualTo(0.5433842148912880).Within(0.05));
            Assert.That(result[23], Is.EqualTo(0.5017780549216030).Within(0.05));
            
            Assert.That(result[50], Is.EqualTo(0.0741442015957425).Within(0.009));
            
            Assert.That(result[75], Is.EqualTo(0.0207568945092484).Within(0.006));
            Assert.That(result[85], Is.EqualTo(0.0136661506653688).Within(0.006));
            Assert.That(result[90], Is.EqualTo(0.0112550619601327).Within(0.006));
            
            Assert.That(result[97], Is.EqualTo(0.0087026933539773).Within(0.005));
            Assert.That(result[98], Is.EqualTo(0.0083995375385004).Within(0.005));
            Assert.That(result[99], Is.EqualTo(0.0081094156379928).Within(0.005));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestInverseGammaGeneratorWithRange01()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.InverseGammaA3B05(rng);
            var samples = new double[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.NextNumber(-1.0, 1.0, dist);
            
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(-1.0), "Min out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0), "Max out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestInverseGammaGeneratorWithRange02()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.InverseGammaA3B05(rng);
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
            Assert.Throws<ArgumentNullException>(() => new FastRng.Double.Distributions.InverseGammaA3B05(null));
        }
    }
}