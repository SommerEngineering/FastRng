using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Float;
using NUnit.Framework;

namespace FastRngTests.Float.Distributions
{
    [ExcludeFromCodeCoverage]
    public class ExponentialLa5
    {
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestExponentialDistribution01()
        {
            var dist = new FastRng.Float.Distributions.ExponentialLa5();
            var fqa = new FrequencyAnalysis();
            var rng = new MultiThreadedRng();
            
            for (var n = 0; n < 100_000; n++)
                fqa.CountThis(await rng.NextNumber(dist));
            
            rng.StopProducer();
            var result = fqa.NormalizeAndPlotEvents(TestContext.WriteLine);

            Assert.That(result[0], Is.EqualTo(1.0002177398625f).Within(0.05f));
            Assert.That(result[1], Is.EqualTo(0.951436545064811f).Within(0.05f));
            Assert.That(result[2], Is.EqualTo(0.905034437210948f).Within(0.05f));
            
            Assert.That(result[21], Is.EqualTo(0.35001394450853f).Within(0.05f));
            Assert.That(result[22], Is.EqualTo(0.332943563002074f).Within(0.05f));
            Assert.That(result[23], Is.EqualTo(0.31670571382568f).Within(0.05f));
            
            Assert.That(result[50], Is.EqualTo(0.082102871800213f).Within(0.01f));
            
            Assert.That(result[75], Is.EqualTo(0.023522866606758f).Within(0.01f));
            Assert.That(result[85], Is.EqualTo(0.014267339801329f).Within(0.01f));
            Assert.That(result[90], Is.EqualTo(0.011111415409621f).Within(0.01f));
            
            Assert.That(result[97], Is.EqualTo(0.007830082099077f).Within(0.008f));
            Assert.That(result[98], Is.EqualTo(0.007448204488898f).Within(0.008f));
            Assert.That(result[99], Is.EqualTo(0.007084951269538f).Within(0.008f));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestExponentialGeneratorWithRange01()
        {
            var dist = new FastRng.Float.Distributions.ExponentialLa5();
            var rng = new MultiThreadedRng();
            var samples = new float[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.NextNumber(-1.0f, 1.0f, dist);
            
            rng.StopProducer();
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(-1.0f), "Min out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0f), "Max out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestExponentialGeneratorWithRange02()
        {
            var dist = new FastRng.Float.Distributions.ExponentialLa5();
            var rng = new MultiThreadedRng();
            var samples = new float[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.NextNumber(0.0f, 1.0f, dist);
            
            rng.StopProducer();
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(0.0f), "Min is out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0f), "Max is out of range");
        }

        [Test] [Category(TestCategories.COVER)] [Category(TestCategories.NORMAL)]
        public async Task TestExponentialGeneratorWithRange03()
        {
            var rng = new MultiThreadedRng();
            var dist = new FastRng.Float.Distributions.ExponentialLa5 { Random = rng }; // Test default parameters
            
            var samples = new float[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await dist.GetDistributedValue();
            
            rng.StopProducer();
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(0.0f), "Min is out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0f), "Max is out of range");
        }

        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task NoRandomNumberGenerator01()
        {
            var dist = new FastRng.Float.Distributions.ExponentialLa5();
            Assert.DoesNotThrowAsync(async () => await dist.GetDistributedValue());
            Assert.That(await dist.GetDistributedValue(), Is.NaN);
        }
    }
}