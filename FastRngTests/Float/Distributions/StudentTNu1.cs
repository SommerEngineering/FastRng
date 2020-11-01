using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Float;
using NUnit.Framework;

namespace FastRngTests.Float.Distributions
{
    [ExcludeFromCodeCoverage]
    public class StudentTNu1
    {
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestStudentTDistribution01()
        {
            var dist = new FastRng.Float.Distributions.StudentTNu1();
            var fra = new FrequencyAnalysis();
            var rng = new MultiThreadedRng();
            
            for (var n = 0; n < 100_000; n++)
                fra.CountThis(await rng.NextNumber(dist));
            
            rng.StopProducer();
            var result = fra.NormalizeAndPlotEvents(TestContext.WriteLine);

            Assert.That(result[0], Is.EqualTo(1.000000000f).Within(0.2f));
            Assert.That(result[1], Is.EqualTo(0.999700120f).Within(0.2f));
            Assert.That(result[2], Is.EqualTo(0.999200719f).Within(0.2f));
            
            Assert.That(result[21], Is.EqualTo(0.953929798f).Within(0.2f));
            Assert.That(result[22], Is.EqualTo(0.949852788f).Within(0.2f));
            Assert.That(result[23], Is.EqualTo(0.945631619f).Within(0.2f));
            
            Assert.That(result[50], Is.EqualTo(0.793667169f).Within(0.095f));
            
            Assert.That(result[75], Is.EqualTo(0.633937627f).Within(0.09f));
            Assert.That(result[85], Is.EqualTo(0.574902276f).Within(0.09f));
            Assert.That(result[90], Is.EqualTo(0.547070729f).Within(0.09f));
            
            Assert.That(result[97], Is.EqualTo(0.510150990f).Within(0.09f));
            Assert.That(result[98], Is.EqualTo(0.505075501f).Within(0.09f));
            Assert.That(result[99], Is.EqualTo(0.500050000f).Within(0.09f));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestStudentTGeneratorWithRange01()
        {
            var rng = new MultiThreadedRng();
            var dist = new FastRng.Float.Distributions.StudentTNu1();
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
        public async Task TestStudentTGeneratorWithRange02()
        {
            var rng = new MultiThreadedRng();
            var dist = new FastRng.Float.Distributions.StudentTNu1();
            var samples = new float[1_000];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = await rng.NextNumber(0.0f, 1.0f, dist);
            
            rng.StopProducer();
            Assert.That(samples.Min(), Is.GreaterThanOrEqualTo(0.0f), "Min is out of range");
            Assert.That(samples.Max(), Is.LessThanOrEqualTo(1.0f), "Max is out of range");
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestStudentTGeneratorWithRange03()
        {
            var rng = new MultiThreadedRng();
            var dist = new FastRng.Float.Distributions.StudentTNu1 { Random = rng }; // Test default parameters
            
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
            var dist = new FastRng.Float.Distributions.StudentTNu1();
            Assert.DoesNotThrowAsync(async () => await dist.GetDistributedValue());
            Assert.That(await dist.GetDistributedValue(), Is.NaN);
        }
    }
}