using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Double;
using NUnit.Framework;

namespace FastRngTests.Double.Distributions
{
    [ExcludeFromCodeCoverage]
    public class ChiSquareK10
    {
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestChiSquareDistribution01()
        {
            var dist = new FastRng.Double.Distributions.ChiSquareK10();
            var fqa = new FrequencyAnalysis();
            var rng = new MultiThreadedRng();
            
            for (var n = 0; n < 100_000; n++)
            {
                var value = await rng.NextNumber(dist);
                fqa.CountThis(value);
            }

            rng.StopProducer();
            
            var result = fqa.NormalizeAndPlotEvents(TestContext.WriteLine);

            Assert.That(result[0], Is.EqualTo(0.0000000164021588).Within(0.0000002));
            Assert.That(result[1], Is.EqualTo(0.0000002611256437).Within(0.000003));
            Assert.That(result[2], Is.EqualTo(0.0000013153553250).Within(0.00002));
            
            Assert.That(result[21], Is.EqualTo(0.003459320622874).Within(0.005));
            Assert.That(result[22], Is.EqualTo(0.004111875573379).Within(0.005));
            Assert.That(result[23], Is.EqualTo(0.004850674298859).Within(0.005));
            
            Assert.That(result[50], Is.EqualTo(0.086418773275056).Within(0.05));
            
            Assert.That(result[75], Is.EqualTo(0.376092741436046).Within(0.08));
            Assert.That(result[85], Is.EqualTo(0.586569751611096).Within(0.08));
            Assert.That(result[90], Is.EqualTo(0.717189736168766).Within(0.08));
            
            Assert.That(result[97], Is.EqualTo(0.931477764640217).Within(0.08));
            Assert.That(result[98], Is.EqualTo(0.965244855212136).Within(0.08));
            Assert.That(result[99], Is.EqualTo(0.999827884370044).Within(0.08));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestChiSquareGeneratorWithRange01()
        {
            var dist = new FastRng.Double.Distributions.ChiSquareK10();
            var rng = new MultiThreadedRng();
            var samples = new double[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.NextNumber(-1.0, 1.0, dist);
            
            rng.StopProducer();
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(-1.0), "Min out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0), "Max out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestChiSquareGeneratorWithRange02()
        {
            var dist = new FastRng.Double.Distributions.ChiSquareK10();
            var rng = new MultiThreadedRng();
            var samples = new double[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.NextNumber(0.0, 1.0, dist);
            
            rng.StopProducer();
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(0.0), "Min is out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0), "Max is out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestChiSquareGeneratorWithRange03()
        {
            var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.ChiSquareK10 { Random = rng }; // Test default parameters
            
            var samples = new double[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await dist.GetDistributedValue();
            
            rng.StopProducer();
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(0.0), "Min is out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0), "Max is out of range");
        }

        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task NoRandomNumberGenerator01()
        {
            var dist = new FastRng.Double.Distributions.ChiSquareK10();
            Assert.DoesNotThrowAsync(async () => await dist.GetDistributedValue());
            Assert.That(await dist.GetDistributedValue(), Is.NaN);
        }
    }
}