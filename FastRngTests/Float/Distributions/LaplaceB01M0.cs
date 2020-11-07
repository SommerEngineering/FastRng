using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Float;
using NUnit.Framework;

namespace FastRngTests.Float.Distributions
{
    [ExcludeFromCodeCoverage]
    public class LaplaceB01M0
    {
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestLaplaceDistribution01()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Float.Distributions.LaplaceB01M0(rng);
            var fra = new FrequencyAnalysis();
            
            for (var n = 0; n < 100_000; n++)
                fra.CountThis(await rng.NextNumber(dist));
            
            var result = fra.NormalizeAndPlotEvents(TestContext.WriteLine);

            Assert.That(result[0], Is.EqualTo(1.0000000000000000f).Within(0.05f));
            Assert.That(result[1], Is.EqualTo(0.9048374180359590f).Within(0.05f));
            Assert.That(result[2], Is.EqualTo(0.8187307530779810f).Within(0.05f));
            
            Assert.That(result[21], Is.EqualTo(0.1224564282529820f).Within(0.05f));
            Assert.That(result[22], Is.EqualTo(0.1108031583623340f).Within(0.05f));
            Assert.That(result[23], Is.EqualTo(0.1002588437228040f).Within(0.05f));
            
            Assert.That(result[50], Is.EqualTo(0.0067379469990855f).Within(0.003f));
            
            Assert.That(result[75], Is.EqualTo(0.0005530843701478f).Within(0.0015f));
            Assert.That(result[85], Is.EqualTo(0.0002034683690106f).Within(0.0015f));
            Assert.That(result[90], Is.EqualTo(0.0001234098040867f).Within(0.0015f));
            
            Assert.That(result[97], Is.EqualTo(0.0000612834950532f).Within(0.0002f));
            Assert.That(result[98], Is.EqualTo(0.0000554515994322f).Within(0.0002f));
            Assert.That(result[99], Is.EqualTo(0.0000501746820562f).Within(0.0002f));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestLaplaceGeneratorWithRange01()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Float.Distributions.LaplaceB01M0(rng);
            var samples = new float[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.NextNumber(-1.0f, 1.0f, dist);
            
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(-1.0f), "Min out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0f), "Max out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestLaplaceGeneratorWithRange02()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Float.Distributions.LaplaceB01M0(rng);
            var samples = new float[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.NextNumber(0.0f, 1.0f, dist);
            
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(0.0f), "Min is out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0f), "Max is out of range");
        }

        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void NoRandomNumberGenerator01()
        {
            Assert.Throws<ArgumentNullException>(() => new FastRng.Float.Distributions.LaplaceB01M0(null));
        }
    }
}