using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Double;
using NUnit.Framework;

namespace FastRngTests.Double.Distributions
{
    [ExcludeFromCodeCoverage]
    public class ChiSquareK1
    {
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestChiSquareDistribution01()
        {
            var dist = new FastRng.Double.Distributions.ChiSquareK1();
            var fqa = new FrequencyAnalysis();
            var rng = new MultiThreadedRng();
            
            for (var n = 0; n < 100_000; n++)
            {
                var value = await rng.NextNumber(dist);
                fqa.CountThis(value);
            }

            rng.StopProducer();
            
            var result = fqa.NormalizeAndPlotEvents(TestContext.WriteLine);

            Assert.That(result[0], Is.EqualTo(1.00032041964207).Within(0.004));
            Assert.That(result[1], Is.EqualTo(0.70380551227703).Within(0.05));
            Assert.That(result[2], Is.EqualTo(0.571788691668126).Within(0.05));
            
            Assert.That(result[21], Is.EqualTo(0.192011337664754).Within(0.07));
            Assert.That(result[22], Is.EqualTo(0.186854182385981).Within(0.07));
            Assert.That(result[23], Is.EqualTo(0.182007652359976).Within(0.07));
            
            Assert.That(result[50], Is.EqualTo(0.109088865614875).Within(0.06));
            
            Assert.That(result[75], Is.EqualTo(0.07886274821701).Within(0.02));
            Assert.That(result[85], Is.EqualTo(0.070520397849883).Within(0.02));
            Assert.That(result[90], Is.EqualTo(0.066863009640287).Within(0.02));
            
            Assert.That(result[97], Is.EqualTo(0.062214737436948).Within(0.02));
            Assert.That(result[98], Is.EqualTo(0.061590997922187).Within(0.02));
            Assert.That(result[99], Is.EqualTo(0.060976622578824).Within(0.02));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestChiSquareGeneratorWithRange01()
        {
            var dist = new FastRng.Double.Distributions.ChiSquareK1();
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
            var dist = new FastRng.Double.Distributions.ChiSquareK1();
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
            var dist = new FastRng.Double.Distributions.ChiSquareK1 { Random = rng }; // Test default parameters
            
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
            var dist = new FastRng.Double.Distributions.ChiSquareK1();
            Assert.DoesNotThrowAsync(async () => await dist.GetDistributedValue());
            Assert.That(await dist.GetDistributedValue(), Is.NaN);
        }
    }
}