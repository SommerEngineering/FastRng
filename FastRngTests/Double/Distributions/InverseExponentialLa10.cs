using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Double;
using NUnit.Framework;

namespace FastRngTests.Double.Distributions
{
    [ExcludeFromCodeCoverage]
    public class InverseExponentialLa10
    {
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestExponentialDistribution01()
        {
            var dist = new FastRng.Double.Distributions.InverseExponentialLa10();
            var fqa = new FrequencyAnalysis();
            var rng = new MultiThreadedRng();
            
            for (var n = 0; n < 100_000; n++)
                fqa.CountThis(await rng.NextNumber(dist));
            
            rng.StopProducer();
            var result = fqa.NormalizeAndPlotEvents(TestContext.WriteLine);

            Assert.That(result[0], Is.EqualTo(0.0000501746820562).Within(0.0003));
            Assert.That(result[1], Is.EqualTo(0.0000554515994322).Within(0.0003));
            Assert.That(result[2], Is.EqualTo(0.0000612834950532).Within(0.0003));
            
            Assert.That(result[21], Is.EqualTo(0.00040973497898).Within(0.00045));
            Assert.That(result[22], Is.EqualTo(0.000452827182887).Within(0.00050));
            Assert.That(result[23], Is.EqualTo(0.000500451433441).Within(0.00051));
            
            Assert.That(result[50], Is.EqualTo(0.007446583070924).Within(0.003));
            
            Assert.That(result[75], Is.EqualTo(0.090717953289412).Within(0.02));
            Assert.That(result[85], Is.EqualTo(0.246596963941606).Within(0.05));
            Assert.That(result[90], Is.EqualTo(0.406569659740598).Within(0.08));
            
            Assert.That(result[97], Is.EqualTo(0.81873075307798).Within(0.08));
            Assert.That(result[98], Is.EqualTo(0.904837418035957).Within(0.08));
            Assert.That(result[99], Is.EqualTo(0.999999999999999).Within(0.08));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestExponentialGeneratorWithRange01()
        {
            var dist = new FastRng.Double.Distributions.InverseExponentialLa10();
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
        public async Task TestExponentialGeneratorWithRange02()
        {
            var dist = new FastRng.Double.Distributions.InverseExponentialLa10();
            var rng = new MultiThreadedRng();
            var samples = new double[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.NextNumber(0.0, 1.0, dist);
            
            rng.StopProducer();
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(0.0), "Min is out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0), "Max is out of range");
        }

        [Test] [Category(TestCategories.COVER)] [Category(TestCategories.NORMAL)]
        public async Task TestExponentialGeneratorWithRange03()
        {
            var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.InverseExponentialLa10 { Random = rng }; // Test default parameters
            
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
            var dist = new FastRng.Double.Distributions.InverseExponentialLa10();
            Assert.DoesNotThrowAsync(async () => await dist.GetDistributedValue());
            Assert.That(await dist.GetDistributedValue(), Is.NaN);
        }
    }
}