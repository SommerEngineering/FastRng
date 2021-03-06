using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Double;
using NUnit.Framework;

namespace FastRngTests.Double.Distributions
{
    [ExcludeFromCodeCoverage]
    public class CauchyLorentzX0
    {
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestCauchyDistribution01()
        {
            // The properties of the cauchy distribution cannot be tested by mean, media or variance,  
            // cf. https://en.wikipedia.org/wiki/Cauchy_distribution#Explanation_of_undefined_moments

            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.CauchyLorentzX0(rng);
            var fqa = new FrequencyAnalysis();
            
            for (var n = 0; n < 100_000; n++)
                fqa.CountThis(await dist.NextNumber());
            
            var result = fqa.NormalizeAndPlotEvents(TestContext.WriteLine);
            
            Assert.That(result[0], Is.EqualTo(0.976990739772031).Within(0.06));
            Assert.That(result[1], Is.EqualTo(0.948808314586299).Within(0.06));
            Assert.That(result[2], Is.EqualTo(0.905284997403441).Within(0.06));
            
            Assert.That(result[21], Is.EqualTo(0.168965864241396).Within(0.04));
            Assert.That(result[22], Is.EqualTo(0.156877686354491).Within(0.04));
            Assert.That(result[23], Is.EqualTo(0.145970509936354).Within(0.04));
            
            Assert.That(result[50], Is.EqualTo(0.036533159835978).Within(0.01));
            
            Assert.That(result[75], Is.EqualTo(0.016793067514802).Within(0.01));
            Assert.That(result[85], Is.EqualTo(0.01316382933791).Within(0.005));
            Assert.That(result[90], Is.EqualTo(0.011773781734516).Within(0.005));
            
            Assert.That(result[97], Is.EqualTo(0.010168596941156).Within(0.005));
            Assert.That(result[98], Is.EqualTo(0.009966272570142).Within(0.005));
            Assert.That(result[99], Is.EqualTo(0.00976990739772).Within(0.005));
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
            Assert.Throws<ArgumentNullException>(() => new FastRng.Double.Distributions.CauchyLorentzX0(null));
        }
    }
}