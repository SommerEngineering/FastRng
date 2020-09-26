using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Double;
using FastRng.Double.Distributions;
using NUnit.Framework;

namespace FastRngTests.Double
{
    [ExcludeFromCodeCoverage]
    public class MultiThreadedRngTests
    {
        private readonly IRandom rng = new MultiThreadedRng();
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestRange01Uint()
        {
            var dist = new Uniform();
            for (uint n = 0; n < 1_000_000; n++)
                Assert.That(await rng.NextNumber(n, 100_000 + n, dist), Is.InRange(n, 100_000 + n));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestRange01Ulong()
        {
            var dist = new Uniform();
            for (ulong n = 0; n < 1_000_000; n++)
                Assert.That(await rng.NextNumber(n, 100_000 + n, dist), Is.InRange(n, 100_000 + n));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestRange01Float()
        {
            var dist = new Uniform();
            for (var n = 0f; n < 1e6f; n++)
                Assert.That(await rng.NextNumber(n, 100_000 + n, dist), Is.InRange(n, 100_000 + n));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestRange02Uint()
        {
            var dist = new Uniform();
            Assert.That(await rng.NextNumber(5, 5, dist), Is.EqualTo(5));
            Assert.That(await rng.NextNumber(0, 0, dist), Is.EqualTo(0));
            Assert.That(await rng.NextNumber(3_000_000_000, 3_000_000_000, dist), Is.EqualTo(3_000_000_000));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestRange02Ulong()
        {
            var dist = new Uniform();
            Assert.That(await rng.NextNumber(5UL, 5, dist), Is.EqualTo(5));
            Assert.That(await rng.NextNumber(0UL, 0, dist), Is.EqualTo(0));
            Assert.That(await rng.NextNumber(3_000_000_000UL, 3_000_000_000, dist), Is.EqualTo(3_000_000_000));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestRange02Float()
        {
            var dist = new Uniform();
            Assert.That(await rng.NextNumber(5f, 5f, dist), Is.EqualTo(5));
            Assert.That(await rng.NextNumber(0f, 0f, dist), Is.EqualTo(0));
            Assert.That(await rng.NextNumber(3e9f, 3e9f, dist), Is.EqualTo(3e9f));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestRange03Uint()
        {
            var dist = new Uniform();
            Assert.That(await rng.NextNumber(5, 6, dist), Is.InRange(5, 6));
            Assert.That(await rng.NextNumber(0, 1, dist), Is.InRange(0, 1));
            Assert.That(await rng.NextNumber(3_000_000_000, 3_000_000_002, dist), Is.InRange(3_000_000_000, 3_000_000_002));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestRange03Ulong()
        {
            var dist = new Uniform();
            Assert.That(await rng.NextNumber(5UL, 6, dist), Is.InRange(5, 6));
            Assert.That(await rng.NextNumber(0UL, 1, dist), Is.InRange(0, 1));
            Assert.That(await rng.NextNumber(3_000_000_000UL, 3_000_000_002, dist), Is.InRange(3_000_000_000, 3_000_000_002));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestRange03Float()
        {
            var dist = new Uniform();
            Assert.That(await rng.NextNumber(5f, 6, dist), Is.InRange(5, 6));
            Assert.That(await rng.NextNumber(0f, 1, dist), Is.InRange(0, 1));
            Assert.That(await rng.NextNumber(3e9f, 3e9f+2, dist), Is.InRange(3e9f, 3e9f+2));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestRange04Uint()
        {
            var dist = new Uniform();
            Assert.That(await rng.NextNumber(10, 1, dist), Is.InRange(1, 10));
            Assert.That(await rng.NextNumber(20, 1, dist), Is.InRange(1, 20));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestRange04Ulong()
        {
            var dist = new Uniform();
            Assert.That(await rng.NextNumber(10UL, 1, dist), Is.InRange(1, 10));
            Assert.That(await rng.NextNumber(20UL, 1, dist), Is.InRange(1, 20));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestRange04Float()
        {
            var dist = new Uniform();
            Assert.That(await rng.NextNumber(10f, 1, dist), Is.InRange(1, 10));
            Assert.That(await rng.NextNumber(20f, 1, dist), Is.InRange(1, 20));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestRange05Uint()
        {
            var dist = new Uniform();
            var distribution = new uint[101];
            var runs = 1_000_000;
            for (var n = 0; n < runs; n++)
                distribution[await rng.NextNumber(0, 100, dist)]++;
            
            for (var n = 0; n < distribution.Length - 1; n++)
                Assert.That(distribution[n], Is.GreaterThan(0));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestRange05Ulong()
        {
            var dist = new Uniform();
            var distribution = new uint[101];
            var runs = 1_000_000;
            for (var n = 0; n < runs; n++)
                distribution[await rng.NextNumber(0UL, 100, dist)]++;
            
            for (var n = 0; n < distribution.Length - 1; n++)
                Assert.That(distribution[n], Is.GreaterThan(0));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestRange05Float()
        {
            var dist = new Uniform();
            var distribution = new uint[101];
            var runs = 1_000_000;
            for (var n = 0; n < runs; n++)
                distribution[(uint)MathF.Floor(await rng.NextNumber(0f, 100f, dist))]++;
            
            for (var n = 0; n < distribution.Length - 1; n++)
                Assert.That(distribution[n], Is.GreaterThan(0));
        }
        
        [Test]
        [Category(TestCategories.NORMAL)]
        public async Task TestDistribution001Uint()
        {
            var dist = new Uniform();
            var distribution = new uint[101];
            var runs = 1_000_000;
            for (var n = 0; n < runs; n++)
                distribution[await rng.NextNumber(0, 100, dist)]++;
            
            Assert.That(distribution[..^1].Max() - distribution[..^1].Min(), Is.InRange(0, 600));
        }
        
        [Test]
        [Category(TestCategories.NORMAL)]
        public async Task TestDistribution001Ulong()
        {
            var dist = new Uniform();
            var distribution = new uint[101];
            var runs = 1_000_000;
            for (var n = 0; n < runs; n++)
                distribution[await rng.NextNumber(0UL, 100, dist)]++;
            
            Assert.That(distribution[..^1].Max() - distribution[..^1].Min(), Is.InRange(0, 600));
        }
        
        [Test]
        [Category(TestCategories.NORMAL)]
        public async Task TestDistribution001Float()
        {
            var dist = new Uniform();
            var distribution = new uint[101];
            var runs = 1_000_000;
            for (var n = 0; n < runs; n++)
                distribution[(uint)MathF.Floor(await rng.NextNumber(0f, 100f, dist))]++;
            
            Assert.That(distribution[..^1].Max() - distribution[..^1].Min(), Is.InRange(0, 600));
        }
        
        [Test]
        [Category(TestCategories.LONG_RUNNING)]
        public async Task TestDistribution002Uint()
        {
            var dist = new Uniform();
            var distribution = new uint[101];
            var runs = 100_000_000;
            for (var n = 0; n < runs; n++)
                distribution[await rng.NextNumber(0, 100, dist)]++;
            
            Assert.That(distribution[..^1].Max() - distribution[..^1].Min(), Is.InRange(0, 6_000));
        }
        
        [Test]
        [Category(TestCategories.LONG_RUNNING)]
        public async Task TestDistribution002Ulong()
        {
            var dist = new Uniform();
            var distribution = new uint[101];
            var runs = 100_000_000;
            for (var n = 0; n < runs; n++)
                distribution[await rng.NextNumber(0UL, 100, dist)]++;
            
            Assert.That(distribution[..^1].Max() - distribution[..^1].Min(), Is.InRange(0, 6_000));
        }
        
        [Test]
        [Category(TestCategories.LONG_RUNNING)]
        public async Task TestDistribution002Float()
        {
            var dist = new Uniform();
            var distribution = new uint[101];
            var runs = 100_000_000;
            for (var n = 0; n < runs; n++)
                distribution[(uint)MathF.Floor(await rng.NextNumber(0f, 100f, dist))]++;
            
            Assert.That(distribution[..^1].Max() - distribution[..^1].Min(), Is.InRange(0, 6_000));
        }
    }
}