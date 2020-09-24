using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng;
using NUnit.Framework;

namespace FastRngTests
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
            for (uint n = 0; n < 1_000_000; n++)
                Assert.That(await rng.NextNumber(n, 100_000 + n), Is.InRange(n, 100_000 + n));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestRange01Ulong()
        {
            for (ulong n = 0; n < 1_000_000; n++)
                Assert.That(await rng.NextNumber(n, 100_000 + n), Is.InRange(n, 100_000 + n));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestRange01Float()
        {
            for (var n = 0f; n < 1e6f; n++)
                Assert.That(await rng.NextNumber(n, 100_000 + n), Is.InRange(n, 100_000 + n));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestRange02Uint()
        {
            Assert.That(await rng.NextNumber(5, 5), Is.EqualTo(5));
            Assert.That(await rng.NextNumber(0, 0), Is.EqualTo(0));
            Assert.That(await rng.NextNumber(3_000_000_000, 3_000_000_000), Is.EqualTo(3_000_000_000));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestRange02Ulong()
        {
            Assert.That(await rng.NextNumber(5UL, 5), Is.EqualTo(5));
            Assert.That(await rng.NextNumber(0UL, 0), Is.EqualTo(0));
            Assert.That(await rng.NextNumber(3_000_000_000UL, 3_000_000_000), Is.EqualTo(3_000_000_000));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestRange02Float()
        {
            Assert.That(await rng.NextNumber(5f, 5f), Is.EqualTo(5));
            Assert.That(await rng.NextNumber(0f, 0f), Is.EqualTo(0));
            Assert.That(await rng.NextNumber(3e9f, 3e9f), Is.EqualTo(3e9f));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestRange03Uint()
        {
            Assert.That(await rng.NextNumber(5, 6), Is.InRange(5, 6));
            Assert.That(await rng.NextNumber(0, 1), Is.InRange(0, 1));
            Assert.That(await rng.NextNumber(3_000_000_000, 3_000_000_002), Is.InRange(3_000_000_000, 3_000_000_002));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestRange03Ulong()
        {
            Assert.That(await rng.NextNumber(5UL, 6), Is.InRange(5, 6));
            Assert.That(await rng.NextNumber(0UL, 1), Is.InRange(0, 1));
            Assert.That(await rng.NextNumber(3_000_000_000UL, 3_000_000_002), Is.InRange(3_000_000_000, 3_000_000_002));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestRange03Float()
        {
            Assert.That(await rng.NextNumber(5f, 6), Is.InRange(5, 6));
            Assert.That(await rng.NextNumber(0f, 1), Is.InRange(0, 1));
            Assert.That(await rng.NextNumber(3e9f, 3e9f+2), Is.InRange(3e9f, 3e9f+2));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestRange04Uint()
        {
            Assert.That(await rng.NextNumber(10, 1), Is.InRange(1, 10));
            Assert.That(await rng.NextNumber(20, 1), Is.InRange(1, 20));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestRange04Ulong()
        {
            Assert.That(await rng.NextNumber(10UL, 1), Is.InRange(1, 10));
            Assert.That(await rng.NextNumber(20UL, 1), Is.InRange(1, 20));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestRange04Float()
        {
            Assert.That(await rng.NextNumber(10f, 1), Is.InRange(1, 10));
            Assert.That(await rng.NextNumber(20f, 1), Is.InRange(1, 20));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestRange05Uint()
        {
            var distribution = new uint[101];
            var runs = 1_000_000;
            for (var n = 0; n < runs; n++)
                distribution[await rng.NextNumber(0, 100)]++;
            
            for (var n = 0; n < distribution.Length - 1; n++)
                Assert.That(distribution[n], Is.GreaterThan(0));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestRange05Ulong()
        {
            var distribution = new uint[101];
            var runs = 1_000_000;
            for (var n = 0; n < runs; n++)
                distribution[await rng.NextNumber(0UL, 100)]++;
            
            for (var n = 0; n < distribution.Length - 1; n++)
                Assert.That(distribution[n], Is.GreaterThan(0));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestRange05Float()
        {
            var distribution = new uint[101];
            var runs = 1_000_000;
            for (var n = 0; n < runs; n++)
                distribution[(uint)MathF.Floor(await rng.NextNumber(0f, 100f))]++;
            
            for (var n = 0; n < distribution.Length - 1; n++)
                Assert.That(distribution[n], Is.GreaterThan(0));
        }
        
        [Test]
        [Category(TestCategories.NORMAL)]
        public async Task TestDistribution001Uint()
        {
            var distribution = new uint[101];
            var runs = 1_000_000;
            for (var n = 0; n < runs; n++)
                distribution[await rng.NextNumber(0, 100)]++;
            
            Assert.That(distribution[..100].Max() - distribution[..100].Min(), Is.InRange(0, 600));
        }
        
        [Test]
        [Category(TestCategories.NORMAL)]
        public async Task TestDistribution001Ulong()
        {
            var distribution = new uint[101];
            var runs = 1_000_000;
            for (var n = 0; n < runs; n++)
                distribution[await rng.NextNumber(0UL, 100)]++;
            
            Assert.That(distribution[..100].Max() - distribution[..100].Min(), Is.InRange(0, 600));
        }
        
        [Test]
        [Category(TestCategories.NORMAL)]
        public async Task TestDistribution001Float()
        {
            var distribution = new uint[101];
            var runs = 1_000_000;
            for (var n = 0; n < runs; n++)
                distribution[(uint)MathF.Floor(await rng.NextNumber(0f, 100f))]++;
            
            Assert.That(distribution[..100].Max() - distribution[..100].Min(), Is.InRange(0, 600));
        }
        
        [Test]
        [Category(TestCategories.LONG_RUNNING)]
        public async Task TestDistribution002Uint()
        {
            var distribution = new uint[101];
            var runs = 100_000_000;
            for (var n = 0; n < runs; n++)
                distribution[await rng.NextNumber(0, 100)]++;
            
            Assert.That(distribution[..100].Max() - distribution[..100].Min(), Is.InRange(0, 600));
        }
        
        [Test]
        [Category(TestCategories.LONG_RUNNING)]
        public async Task TestDistribution002Ulong()
        {
            var distribution = new uint[101];
            var runs = 100_000_000;
            for (var n = 0; n < runs; n++)
                distribution[await rng.NextNumber(0UL, 100)]++;
            
            Assert.That(distribution[..100].Max() - distribution[..100].Min(), Is.InRange(0, 600));
        }
        
        [Test]
        [Category(TestCategories.LONG_RUNNING)]
        public async Task TestDistribution002Float()
        {
            var distribution = new uint[101];
            var runs = 100_000_000;
            for (var n = 0; n < runs; n++)
                distribution[(uint)MathF.Floor(await rng.NextNumber(0f, 100f))]++;
            
            Assert.That(distribution[..100].Max() - distribution[..100].Min(), Is.InRange(0, 600));
        }
    }
}