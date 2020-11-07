using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Double;
using NUnit.Framework;

namespace FastRngTests.Double.Distributions
{
    [ExcludeFromCodeCoverage]
    public class InverseExponentialLa5
    {
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestExponentialDistribution01()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.InverseExponentialLa5(rng);
            var fqa = new FrequencyAnalysis();
            
            for (var n = 0; n < 100_000; n++)
                fqa.CountThis(await dist.NextNumber());
            
            var result = fqa.NormalizeAndPlotEvents(TestContext.WriteLine);

            Assert.That(result[0], Is.EqualTo(0.007083408929052).Within(0.008));
            Assert.That(result[1], Is.EqualTo(0.007446583070924).Within(0.008));
            Assert.That(result[2], Is.EqualTo(0.007828377549226).Within(0.008));
            
            Assert.That(result[21], Is.EqualTo(0.020241911445804).Within(0.05));
            Assert.That(result[22], Is.EqualTo(0.021279736438377).Within(0.05));
            Assert.That(result[23], Is.EqualTo(0.022370771856166).Within(0.05));
            
            Assert.That(result[50], Is.EqualTo(0.08629358649937).Within(0.02));
            
            Assert.That(result[75], Is.EqualTo(0.301194211912202).Within(0.03));
            Assert.That(result[85], Is.EqualTo(0.496585303791409).Within(0.05));
            Assert.That(result[90], Is.EqualTo(0.637628151621772).Within(0.06));
            
            Assert.That(result[97], Is.EqualTo(0.904837418035959).Within(0.08));
            Assert.That(result[98], Is.EqualTo(0.951229424500713).Within(0.08));
            Assert.That(result[99], Is.EqualTo(1).Within(0.08));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestExponentialGeneratorWithRange01()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.InverseExponentialLa5(rng);
            var samples = new double[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await dist.NextNumber(-1.0, 1.0);
            
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(-1.0), "Min out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0), "Max out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestExponentialGeneratorWithRange02()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.InverseExponentialLa5(rng);
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
            Assert.Throws<ArgumentNullException>(() => new FastRng.Double.Distributions.InverseExponentialLa5(null));
        }
    }
}