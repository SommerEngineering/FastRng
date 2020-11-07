using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Double;
using NUnit.Framework;

namespace FastRngTests.Double.Distributions
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
            var dist = new FastRng.Double.Distributions.LaplaceB01M05(rng);
            var fra = new FrequencyAnalysis();
            
            for (var n = 0; n < 100_000; n++)
                fra.CountThis(await dist.NextNumber());
            
            var result = fra.NormalizeAndPlotEvents(TestContext.WriteLine);

            Assert.That(result[0], Is.EqualTo(0.0074465830709244).Within(0.004));
            Assert.That(result[1], Is.EqualTo(0.0082297470490200).Within(0.004));
            Assert.That(result[2], Is.EqualTo(0.0090952771016958).Within(0.01));
            
            Assert.That(result[21], Is.EqualTo(0.0608100626252180).Within(0.02));
            Assert.That(result[22], Is.EqualTo(0.0672055127397498).Within(0.02));
            Assert.That(result[23], Is.EqualTo(0.0742735782143340).Within(0.02));
            
            Assert.That(result[50], Is.EqualTo(1.0000000000000000).Within(0.2));
            
            Assert.That(result[75], Is.EqualTo(0.0742735782143335).Within(0.01));
            Assert.That(result[85], Is.EqualTo(0.0273237224472924).Within(0.01));
            Assert.That(result[90], Is.EqualTo(0.0165726754017612).Within(0.01));
            
            Assert.That(result[97], Is.EqualTo(0.0082297470490200).Within(0.004));
            Assert.That(result[98], Is.EqualTo(0.0074465830709243).Within(0.004));
            Assert.That(result[99], Is.EqualTo(0.0067379469990854).Within(0.004));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestLaplaceGeneratorWithRange01()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.LaplaceB01M05(rng);
            var samples = new double[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await dist.NextNumber(-1.0, 1.0);
            
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(-1.0), "Min out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0), "Max out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestLaplaceGeneratorWithRange02()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.LaplaceB01M05(rng);
            var samples = new double[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await dist.NextNumber(0.0, 1.0);
            
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(0.0), "Min is out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0), "Max is out of range");
        }

        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void NoRandomNumberGenerator01()
        {
            Assert.Throws<ArgumentNullException>(() => new FastRng.Double.Distributions.LaplaceB01M05(null));
        }
    }
}