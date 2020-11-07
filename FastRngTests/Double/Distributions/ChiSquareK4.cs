using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Double;
using NUnit.Framework;

namespace FastRngTests.Double.Distributions
{
    [ExcludeFromCodeCoverage]
    public class ChiSquareK4
    {
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestChiSquareDistribution01()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.ChiSquareK4(rng);
            var fqa = new FrequencyAnalysis();
            
            for (var n = 0; n < 100_000; n++)
                fqa.CountThis(await rng.NextNumber(dist));
            
            var result = fqa.NormalizeAndPlotEvents(TestContext.WriteLine);

            Assert.That(result[0], Is.EqualTo(0.016417705906679).Within(0.02));
            Assert.That(result[1], Is.EqualTo(0.032671644513723).Within(0.02));
            Assert.That(result[2], Is.EqualTo(0.048763041010352).Within(0.02));
            
            Assert.That(result[21], Is.EqualTo(0.32518779111264).Within(0.05));
            Assert.That(result[22], Is.EqualTo(0.338273451612642).Within(0.05));
            Assert.That(result[23], Is.EqualTo(0.351220492939994).Within(0.05));
            
            Assert.That(result[50], Is.EqualTo(0.65209223303425).Within(0.08));
            
            Assert.That(result[75], Is.EqualTo(0.857562207152294).Within(0.099));
            Assert.That(result[85], Is.EqualTo(0.923072405412387).Within(0.099));
            Assert.That(result[90], Is.EqualTo(0.952623623874265).Within(0.099));
            
            Assert.That(result[97], Is.EqualTo(0.990616879396201).Within(0.099));
            Assert.That(result[98], Is.EqualTo(0.995734077068522).Within(0.099));
            Assert.That(result[99], Is.EqualTo(1.00077558852585).Within(0.1));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestChiSquareGeneratorWithRange01()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.ChiSquareK4(rng);
            var samples = new double[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.NextNumber(-1.0, 1.0, dist);
            
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(-1.0), "Min out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0), "Max out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestChiSquareGeneratorWithRange02()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.ChiSquareK4(rng);
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
            Assert.Throws<ArgumentNullException>(() => new FastRng.Double.Distributions.ChiSquareK4(null));
        }
    }
}