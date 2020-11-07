using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Float;
using NUnit.Framework;

namespace FastRngTests.Float.Distributions
{
    [ExcludeFromCodeCoverage]
    public class ExponentialLa10
    {
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestExponentialDistribution01()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Float.Distributions.ExponentialLa10(rng);
            var fqa = new FrequencyAnalysis();
            
            for (var n = 0; n < 100_000; n++)
                fqa.CountThis(await dist.NextNumber());
            
            var result = fqa.NormalizeAndPlotEvents(TestContext.WriteLine);

            Assert.That(result[0], Is.EqualTo(1.00075018434777f).Within(0.05f));
            Assert.That(result[1], Is.EqualTo(0.905516212904248f).Within(0.05f));
            Assert.That(result[2], Is.EqualTo(0.81934495207398f).Within(0.05f));
            
            Assert.That(result[21], Is.EqualTo(0.122548293148741f).Within(0.12f));
            Assert.That(result[22], Is.EqualTo(0.110886281157421f).Within(0.12f));
            Assert.That(result[23], Is.EqualTo(0.10033405633809f).Within(0.12f));
            
            Assert.That(result[50], Is.EqualTo(0.00674300170146f).Within(0.005f));
            
            Assert.That(result[75], Is.EqualTo(0.000553499285385f).Within(0.001f));
            Assert.That(result[85], Is.EqualTo(0.000203621007796f).Within(0.001f));
            Assert.That(result[90], Is.EqualTo(0.00012350238419f).Within(0.001f));
            
            Assert.That(result[97], Is.EqualTo(0.0000613294689720f).Within(0.0008f));
            Assert.That(result[98], Is.EqualTo(0.0000554931983541f).Within(0.0008f));
            Assert.That(result[99], Is.EqualTo(0.0000502123223173f).Within(0.0008f));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestExponentialGeneratorWithRange01()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Float.Distributions.ExponentialLa10(rng);
            var samples = new float[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await dist.NextNumber(-1.0f, 1.0f);
            
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(-1.0f), "Min out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0f), "Max out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestExponentialGeneratorWithRange02()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Float.Distributions.ExponentialLa10(rng);
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
            Assert.Throws<ArgumentNullException>(() => new FastRng.Float.Distributions.ExponentialLa10(null));
        }
    }
}