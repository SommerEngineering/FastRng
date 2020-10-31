using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Double;
using NUnit.Framework;

namespace FastRngTests.Double.Distributions
{
    [ExcludeFromCodeCoverage]
    public class LaplaceB01M0
    {
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestLaplaceDistribution01()
        {
            var dist = new FastRng.Double.Distributions.LaplaceB01M0();
            var fra = new FrequencyAnalysis();
            var rng = new MultiThreadedRng();
            
            for (var n = 0; n < 100_000; n++)
                fra.CountThis(await rng.NextNumber(dist));
            
            rng.StopProducer();
            var result = fra.NormalizeAndPlotEvents(TestContext.WriteLine);

            Assert.That(result[0], Is.EqualTo(1.0000000000000000).Within(0.05));
            Assert.That(result[1], Is.EqualTo(0.9048374180359590).Within(0.05));
            Assert.That(result[2], Is.EqualTo(0.8187307530779810).Within(0.05));
            
            Assert.That(result[21], Is.EqualTo(0.1224564282529820).Within(0.05));
            Assert.That(result[22], Is.EqualTo(0.1108031583623340).Within(0.05));
            Assert.That(result[23], Is.EqualTo(0.1002588437228040).Within(0.05));
            
            Assert.That(result[50], Is.EqualTo(0.0067379469990855).Within(0.003));
            
            Assert.That(result[75], Is.EqualTo(0.0005530843701478).Within(0.0015));
            Assert.That(result[85], Is.EqualTo(0.0002034683690106).Within(0.0015));
            Assert.That(result[90], Is.EqualTo(0.0001234098040867).Within(0.0015));
            
            Assert.That(result[97], Is.EqualTo(0.0000612834950532).Within(0.0002));
            Assert.That(result[98], Is.EqualTo(0.0000554515994322).Within(0.0002));
            Assert.That(result[99], Is.EqualTo(0.0000501746820562).Within(0.0002));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestLaplaceGeneratorWithRange01()
        {
            var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.LaplaceB01M0();
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
        public async Task TestLaplaceGeneratorWithRange02()
        {
            var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.LaplaceB01M0();
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
        public async Task TestLaplaceGeneratorWithRange03()
        {
            var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.LaplaceB01M0 { Random = rng }; // Test default parameters
            
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
            var dist = new FastRng.Double.Distributions.LaplaceB01M0();
            Assert.DoesNotThrowAsync(async () => await dist.GetDistributedValue());
            Assert.That(await dist.GetDistributedValue(), Is.NaN);
        }
    }
}