using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Double;
using NUnit.Framework;

namespace FastRngTests.Double.Distributions
{
    [ExcludeFromCodeCoverage]
    public class StudentTNu1
    {
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestStudentTDistribution01()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.StudentTNu1(rng);
            var fra = new FrequencyAnalysis();
            
            for (var n = 0; n < 100_000; n++)
                fra.CountThis(await dist.NextNumber());
            
            var result = fra.NormalizeAndPlotEvents(TestContext.WriteLine);

            Assert.That(result[0], Is.EqualTo(1.000000000).Within(0.2));
            Assert.That(result[1], Is.EqualTo(0.999700120).Within(0.2));
            Assert.That(result[2], Is.EqualTo(0.999200719).Within(0.2));
            
            Assert.That(result[21], Is.EqualTo(0.953929798).Within(0.2));
            Assert.That(result[22], Is.EqualTo(0.949852788).Within(0.2));
            Assert.That(result[23], Is.EqualTo(0.945631619).Within(0.2));
            
            Assert.That(result[50], Is.EqualTo(0.793667169).Within(0.095));
            
            Assert.That(result[75], Is.EqualTo(0.633937627).Within(0.09));
            Assert.That(result[85], Is.EqualTo(0.574902276).Within(0.09));
            Assert.That(result[90], Is.EqualTo(0.547070729).Within(0.09));
            
            Assert.That(result[97], Is.EqualTo(0.510150990).Within(0.09));
            Assert.That(result[98], Is.EqualTo(0.505075501).Within(0.09));
            Assert.That(result[99], Is.EqualTo(0.500050000).Within(0.09));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestStudentTGeneratorWithRange01()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.StudentTNu1(rng);
            var samples = new double[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await dist.NextNumber(-1.0, 1.0);
            
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(-1.0), "Min out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0), "Max out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestStudentTGeneratorWithRange02()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.StudentTNu1(rng);
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
            Assert.Throws<ArgumentNullException>(() => new FastRng.Double.Distributions.StudentTNu1(null));
        }
    }
}