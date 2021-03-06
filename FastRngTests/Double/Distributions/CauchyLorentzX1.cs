using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Double;
using NUnit.Framework;

namespace FastRngTests.Double.Distributions
{
    [ExcludeFromCodeCoverage]
    public class CauchyLorentzX1
    {
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestCauchyDistribution01()
        {
            // The properties of the cauchy distribution cannot be tested by mean, media or variance,  
            // cf. https://en.wikipedia.org/wiki/Cauchy_distribution#Explanation_of_undefined_moments

            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.CauchyLorentzX1(rng);
            var fqa = new FrequencyAnalysis();
            
            for (var n = 0; n < 100_000; n++)
                fqa.CountThis(await dist.NextNumber());
            
            var result = fqa.NormalizeAndPlotEvents(TestContext.WriteLine);
            
            Assert.That(result[0], Is.EqualTo(0.009966272570142).Within(0.003));
            Assert.That(result[1], Is.EqualTo(0.010168596941156).Within(0.004));
            Assert.That(result[2], Is.EqualTo(0.010377123221893).Within(0.005));
            
            Assert.That(result[21], Is.EqualTo(0.015956672819692).Within(0.005));
            Assert.That(result[22], Is.EqualTo(0.016366904083094).Within(0.005));
            Assert.That(result[23], Is.EqualTo(0.016793067514802).Within(0.005));
            
            Assert.That(result[50], Is.EqualTo(0.039454644029179).Within(0.015));
            
            Assert.That(result[75], Is.EqualTo(0.145970509936354).Within(0.03));
            Assert.That(result[85], Is.EqualTo(0.333365083503296).Within(0.1));
            Assert.That(result[90], Is.EqualTo(0.545171628270584).Within(0.1));
            
            Assert.That(result[97], Is.EqualTo(0.948808314586302).Within(0.06));
            Assert.That(result[98], Is.EqualTo(0.976990739772032).Within(0.03));
            Assert.That(result[99], Is.EqualTo(0.986760647169751).Within(0.02));
        }

        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestCauchyGeneratorWithRange01()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.CauchyLorentzX0(rng);
            var samples = new double[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await dist.NextNumber(-1.0, 1.0);
            
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(-1.0), "Min is out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0), "Max is out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestCauchyGeneratorWithRange02()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.CauchyLorentzX0(rng);
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
            Assert.Throws<ArgumentNullException>(() => new FastRng.Double.Distributions.CauchyLorentzX1(null));
        }
    }
}