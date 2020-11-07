using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Float;
using NUnit.Framework;

namespace FastRngTests.Float.Distributions
{
    [ExcludeFromCodeCoverage]
    public class LaplaceB01M05
    {
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestLaplaceDistribution01()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Float.Distributions.LaplaceB01M05(rng);
            var fra = new FrequencyAnalysis();
            
            for (var n = 0; n < 100_000; n++)
                fra.CountThis(await dist.NextNumber());
            
            var result = fra.NormalizeAndPlotEvents(TestContext.WriteLine);

            Assert.That(result[0], Is.EqualTo(0.0074465830709244f).Within(0.004f));
            Assert.That(result[1], Is.EqualTo(0.0082297470490200f).Within(0.004f));
            Assert.That(result[2], Is.EqualTo(0.0090952771016958f).Within(0.01f));
            
            Assert.That(result[21], Is.EqualTo(0.0608100626252180f).Within(0.02f));
            Assert.That(result[22], Is.EqualTo(0.0672055127397498f).Within(0.02f));
            Assert.That(result[23], Is.EqualTo(0.0742735782143340f).Within(0.02f));
            
            Assert.That(result[50], Is.EqualTo(1.0000000000000000f).Within(0.2f));
            
            Assert.That(result[75], Is.EqualTo(0.0742735782143335f).Within(0.01f));
            Assert.That(result[85], Is.EqualTo(0.0273237224472924f).Within(0.01f));
            Assert.That(result[90], Is.EqualTo(0.0165726754017612f).Within(0.01f));
            
            Assert.That(result[97], Is.EqualTo(0.0082297470490200f).Within(0.004f));
            Assert.That(result[98], Is.EqualTo(0.0074465830709243f).Within(0.004f));
            Assert.That(result[99], Is.EqualTo(0.0067379469990854f).Within(0.004f));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestLaplaceGeneratorWithRange01()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Float.Distributions.LaplaceB01M05(rng);
            var samples = new float[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await dist.NextNumber(-1.0f, 1.0f);
            
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(-1.0f), "Min out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0f), "Max out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestLaplaceGeneratorWithRange02()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Float.Distributions.LaplaceB01M05(rng);
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
            Assert.Throws<ArgumentNullException>(() => new FastRng.Float.Distributions.LaplaceB01M05(null));
        }
    }
}