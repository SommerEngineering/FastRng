using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Float;
using NUnit.Framework;

namespace FastRngTests.Float.Distributions
{
    [ExcludeFromCodeCoverage]
    public class ChiSquareK10
    {
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestChiSquareDistribution01()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Float.Distributions.ChiSquareK10(rng);
            var fqa = new FrequencyAnalysis();

            for (var n = 0; n < 100_000; n++)
            {
                var value = await dist.NextNumber();
                fqa.CountThis(value);
            }
            
            var result = fqa.NormalizeAndPlotEvents(TestContext.WriteLine);

            Assert.That(result[0], Is.EqualTo(0.0000000164021588f).Within(0.0000002f));
            Assert.That(result[1], Is.EqualTo(0.0000002611256437f).Within(0.000003f));
            Assert.That(result[2], Is.EqualTo(0.0000013153553250f).Within(0.00002f));
            
            Assert.That(result[21], Is.EqualTo(0.003459320622874f).Within(0.005f));
            Assert.That(result[22], Is.EqualTo(0.004111875573379f).Within(0.005f));
            Assert.That(result[23], Is.EqualTo(0.004850674298859f).Within(0.005f));
            
            Assert.That(result[50], Is.EqualTo(0.086418773275056f).Within(0.05f));
            
            Assert.That(result[75], Is.EqualTo(0.376092741436046f).Within(0.08f));
            Assert.That(result[85], Is.EqualTo(0.586569751611096f).Within(0.08f));
            Assert.That(result[90], Is.EqualTo(0.717189736168766f).Within(0.08f));
            
            Assert.That(result[97], Is.EqualTo(0.931477764640217f).Within(0.08f));
            Assert.That(result[98], Is.EqualTo(0.965244855212136f).Within(0.08f));
            Assert.That(result[99], Is.EqualTo(0.999827884370044f).Within(0.08f));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestChiSquareGeneratorWithRange01()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Float.Distributions.ChiSquareK10(rng);
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
            var dist = new FastRng.Float.Distributions.ChiSquareK10(rng);
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
            Assert.Throws<ArgumentNullException>(() => new FastRng.Float.Distributions.ChiSquareK10(null));
        }
    }
}