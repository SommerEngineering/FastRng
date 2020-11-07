using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Float;
using NUnit.Framework;

namespace FastRngTests.Float.Distributions
{
    [ExcludeFromCodeCoverage]
    public class ChiSquareK1
    {
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestChiSquareDistribution01()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Float.Distributions.ChiSquareK1(rng);
            var fqa = new FrequencyAnalysis();

            for (var n = 0; n < 100_000; n++)
            {
                var value = await dist.NextNumber();
                fqa.CountThis(value);
            }
            
            var result = fqa.NormalizeAndPlotEvents(TestContext.WriteLine);

            Assert.That(result[0], Is.EqualTo(1.00032041964207f).Within(0.004f));
            Assert.That(result[1], Is.EqualTo(0.70380551227703f).Within(0.05f));
            Assert.That(result[2], Is.EqualTo(0.571788691668126f).Within(0.05f));
            
            Assert.That(result[21], Is.EqualTo(0.192011337664754f).Within(0.07f));
            Assert.That(result[22], Is.EqualTo(0.186854182385981f).Within(0.07f));
            Assert.That(result[23], Is.EqualTo(0.182007652359976f).Within(0.07f));
            
            Assert.That(result[50], Is.EqualTo(0.109088865614875f).Within(0.06f));
            
            Assert.That(result[75], Is.EqualTo(0.07886274821701f).Within(0.02f));
            Assert.That(result[85], Is.EqualTo(0.070520397849883f).Within(0.02f));
            Assert.That(result[90], Is.EqualTo(0.066863009640287f).Within(0.02f));
            
            Assert.That(result[97], Is.EqualTo(0.062214737436948f).Within(0.02f));
            Assert.That(result[98], Is.EqualTo(0.061590997922187f).Within(0.02f));
            Assert.That(result[99], Is.EqualTo(0.060976622578824f).Within(0.02f));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestChiSquareGeneratorWithRange01()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Float.Distributions.ChiSquareK1(rng);
            var samples = new float[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await dist.NextNumber(-1.0f, 1.0f);
            
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(-1.0f), "Min out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0f), "Max out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestChiSquareGeneratorWithRange02()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Float.Distributions.ChiSquareK1(rng);
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
            Assert.Throws<ArgumentNullException>(() => new FastRng.Float.Distributions.ChiSquareK1(null));
        }
    }
}