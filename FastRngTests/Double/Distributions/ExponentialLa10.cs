using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Double;
using NUnit.Framework;

namespace FastRngTests.Double.Distributions
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
            var dist = new FastRng.Double.Distributions.ExponentialLa10(rng);
            var fqa = new FrequencyAnalysis();
            
            for (var n = 0; n < 100_000; n++)
                fqa.CountThis(await rng.NextNumber(dist));
            
            var result = fqa.NormalizeAndPlotEvents(TestContext.WriteLine);

            Assert.That(result[0], Is.EqualTo(1.00075018434777).Within(0.05));
            Assert.That(result[1], Is.EqualTo(0.905516212904248).Within(0.05));
            Assert.That(result[2], Is.EqualTo(0.81934495207398).Within(0.05));
            
            Assert.That(result[21], Is.EqualTo(0.122548293148741).Within(0.12));
            Assert.That(result[22], Is.EqualTo(0.110886281157421).Within(0.12));
            Assert.That(result[23], Is.EqualTo(0.10033405633809).Within(0.12));
            
            Assert.That(result[50], Is.EqualTo(0.00674300170146).Within(0.005));
            
            Assert.That(result[75], Is.EqualTo(0.000553499285385).Within(0.001));
            Assert.That(result[85], Is.EqualTo(0.000203621007796).Within(0.001));
            Assert.That(result[90], Is.EqualTo(0.00012350238419).Within(0.001));
            
            Assert.That(result[97], Is.EqualTo(0.0000613294689720).Within(0.0008));
            Assert.That(result[98], Is.EqualTo(0.0000554931983541).Within(0.0008));
            Assert.That(result[99], Is.EqualTo(0.0000502123223173).Within(0.0008));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestExponentialGeneratorWithRange01()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.ExponentialLa10(rng);
            var samples = new double[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.NextNumber(-1.0, 1.0, dist);
            
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(-1.0), "Min out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0), "Max out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestExponentialGeneratorWithRange02()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.ExponentialLa10(rng);
            var samples = new double[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.NextNumber(0.0, 1.0, dist);
            
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(0.0), "Min is out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0), "Max is out of range");
        }

        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void NoRandomNumberGenerator01()
        {
            Assert.Throws<ArgumentNullException>(() => new FastRng.Double.Distributions.ExponentialLa10(null));
        }
    }
}