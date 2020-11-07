using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Double;
using NUnit.Framework;

namespace FastRngTests.Double.Distributions
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
            var dist = new FastRng.Double.Distributions.GammaA5B15(rng);
            var fra = new FrequencyAnalysis();
            
            for (var n = 0; n < 100_000; n++)
                fra.CountThis(await rng.NextNumber(dist));
            
            var result = fra.NormalizeAndPlotEvents(TestContext.WriteLine);

            Assert.That(result[0], Is.EqualTo(0.0000929594237282).Within(0.0008));
            Assert.That(result[1], Is.EqualTo(0.0012801746797876).Within(0.002));
            Assert.That(result[2], Is.EqualTo(0.0055781488254349).Within(0.004));
            
            Assert.That(result[21], Is.EqualTo(0.9331608887752720).Within(0.09));
            Assert.That(result[22], Is.EqualTo(0.9594734828891280).Within(0.09));
            Assert.That(result[23], Is.EqualTo(0.9790895765535350).Within(0.09));
            
            Assert.That(result[50], Is.EqualTo(0.3478287795336570).Within(0.06));
            
            Assert.That(result[75], Is.EqualTo(0.0403399049422936).Within(0.009));
            Assert.That(result[85], Is.EqualTo(0.0163628388658126).Within(0.009));
            Assert.That(result[90], Is.EqualTo(0.0097147611446660).Within(0.005));
            
            Assert.That(result[97], Is.EqualTo(0.0041135143233153).Within(0.008));
            Assert.That(result[98], Is.EqualTo(0.0036872732029996).Within(0.008));
            Assert.That(result[99], Is.EqualTo(0.0033038503429554).Within(0.008));
        }

        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestGammaGeneratorWithRange01()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.GammaA5B15(rng);
            var samples = new double[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.NextNumber(-1.0, 1.0, dist);
            
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(-1.0), "Min is out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0), "Max is out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestGammaGeneratorWithRange02()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.GammaA5B15(rng);
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
            Assert.Throws<ArgumentNullException>(() => new FastRng.Double.Distributions.GammaA5B15(null));
        }
    }
}