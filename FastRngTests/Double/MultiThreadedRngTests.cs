using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
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
            var dist = new Uniform(this.rng);
            for (uint n = 0; n < 1_000_000; n++)
                Assert.That(await dist.NextNumber(n, 100_000 + n), Is.InRange(n, 100_000 + n));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestRange01Ulong()
        {
            var dist = new Uniform(this.rng);
            for (ulong n = 0; n < 1_000_000; n++)
                Assert.That(await dist.NextNumber(n, 100_000 + n), Is.InRange(n, 100_000 + n));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestRange01Float()
        {
            var dist = new Uniform(this.rng);
            for (var n = 0.0; n < 1e6; n++)
                Assert.That(await dist.NextNumber(n, 100_000 + n), Is.InRange(n, 100_000 + n));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestRange02Uint()
        {
            var dist = new Uniform(this.rng);
            Assert.That(await dist.NextNumber(5, 5), Is.EqualTo(5));
            Assert.That(await dist.NextNumber(0, 0), Is.EqualTo(0));
            Assert.That(await dist.NextNumber(3_000_000_000, 3_000_000_000), Is.EqualTo(3_000_000_000));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestRange02Ulong()
        {
            var dist = new Uniform(this.rng);
            Assert.That(await dist.NextNumber(5UL, 5), Is.EqualTo(5));
            Assert.That(await dist.NextNumber(0UL, 0), Is.EqualTo(0));
            Assert.That(await dist.NextNumber(3_000_000_000UL, 3_000_000_000), Is.EqualTo(3_000_000_000));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestRange02Float()
        {
            var dist = new Uniform(this.rng);
            Assert.That(await dist.NextNumber(5f, 5f), Is.EqualTo(5));
            Assert.That(await dist.NextNumber(0f, 0f), Is.EqualTo(0));
            Assert.That(await dist.NextNumber(3e9, 3e9), Is.EqualTo(3e9));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestRange03Uint()
        {
            var dist = new Uniform(this.rng);
            Assert.That(await dist.NextNumber(5, 6), Is.InRange(5, 6));
            Assert.That(await dist.NextNumber(0, 1), Is.InRange(0, 1));
            Assert.That(await dist.NextNumber(3_000_000_000, 3_000_000_002), Is.InRange(3_000_000_000, 3_000_000_002));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestRange03Ulong()
        {
            var dist = new Uniform(this.rng);
            Assert.That(await dist.NextNumber(5UL, 6), Is.InRange(5, 6));
            Assert.That(await dist.NextNumber(0UL, 1), Is.InRange(0, 1));
            Assert.That(await dist.NextNumber(3_000_000_000UL, 3_000_000_002), Is.InRange(3_000_000_000, 3_000_000_002));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestRange03Float()
        {
            var dist = new Uniform(this.rng);
            Assert.That(await dist.NextNumber(5f, 6), Is.InRange(5, 6));
            Assert.That(await dist.NextNumber(0f, 1), Is.InRange(0, 1));
            Assert.That(await dist.NextNumber(3e9, 3e9+2), Is.InRange(3e9, 3e9+2));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestRange04Uint()
        {
            var distUniform = new Uniform(this.rng);
            var distNormal = new NormalS02M05(this.rng);
            
            Assert.That(await distUniform.NextNumber(10, 1), Is.InRange(1, 10));
            Assert.That(await distNormal.NextNumber(10, 1), Is.InRange(1, 10));
            
            Assert.That(await distUniform.NextNumber(20, 1), Is.InRange(1, 20));
            Assert.That(await distNormal.NextNumber(20, 1), Is.InRange(1, 20));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestRange04Ulong()
        {
            var distUniform = new Uniform(this.rng);
            var distNormal = new NormalS02M05(this.rng);
            
            Assert.That(await distUniform.NextNumber(10UL, 1), Is.InRange(1, 10));
            Assert.That(await distNormal.NextNumber(10UL, 1), Is.InRange(1, 10));
            
            Assert.That(await distUniform.NextNumber(20UL, 1), Is.InRange(1, 20));
            Assert.That(await distNormal.NextNumber(20UL, 1), Is.InRange(1, 20));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestRange04Float()
        {
            var distUniform = new Uniform(this.rng);
            var distNormal = new NormalS02M05(this.rng);
            
            Assert.That(await distUniform.NextNumber(10.0, 1), Is.InRange(1, 10));
            Assert.That(await distNormal.NextNumber(10.0, 1), Is.InRange(1, 10));
            
            Assert.That(await distUniform.NextNumber(20.0, 1), Is.InRange(1, 20));
            Assert.That(await distNormal.NextNumber(20.0, 1), Is.InRange(1, 20));
        }

        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestStoppingProducers01()
        {
            var rng2 = new MultiThreadedRng();
            rng2.Dispose();

            var masterToken = new CancellationTokenSource(TimeSpan.FromSeconds(16)).Token;
            var wasCanceled = false;
            
            while(true)
            {
                var tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(3));
                await rng2.GetUniform(tokenSource.Token);
                if (tokenSource.IsCancellationRequested)
                {
                    wasCanceled = true;
                    break;
                }

                if (masterToken.IsCancellationRequested)
                {
                    break;
                }
            }
            
            Assert.That(masterToken.IsCancellationRequested, Is.False, "Master token was used to stop test");
            Assert.That(wasCanceled, Is.True, "The consumer was not canceled");
            
            var tokenSource2 = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            await new NormalS02M05(rng2).NextNumber(tokenSource2.Token);
            Assert.That(tokenSource2.IsCancellationRequested, Is.True);
            
            tokenSource2 = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            await new NormalS02M05(rng2).NextNumber(-1d, 1d, tokenSource2.Token);
            Assert.That(tokenSource2.IsCancellationRequested, Is.True);
            
            tokenSource2 = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            await new NormalS02M05(rng2).NextNumber(0u, 6u, tokenSource2.Token);
            Assert.That(tokenSource2.IsCancellationRequested, Is.True);
            
            tokenSource2 = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            await new NormalS02M05(rng2).NextNumber(0ul, 6ul, tokenSource2.Token);
            Assert.That(tokenSource2.IsCancellationRequested, Is.True);
        }

        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task OneSeed01()
        {
            using var rng1 = new MultiThreadedRng(6);
            using var rng2 = new MultiThreadedRng(6);
            using var rng3 = new MultiThreadedRng(7);

            var rng1Sample = new double[10];
            for (var n = 0; n < rng1Sample.Length; n++)
                rng1Sample[n] = await rng1.GetUniform();
            
            var rng2Sample = new double[10];
            for (var n = 0; n < rng2Sample.Length; n++)
                rng2Sample[n] = await rng2.GetUniform();
            
            var rng3Sample = new double[10];
            for (var n = 0; n < rng3Sample.Length; n++)
                rng3Sample[n] = await rng3.GetUniform();

            Assert.That(rng1Sample, Is.EquivalentTo(rng2Sample));
            Assert.That(rng1Sample, Is.Not.EquivalentTo(rng3Sample));
            Assert.That(rng2Sample, Is.Not.EquivalentTo(rng3Sample));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TwoSeeds01()
        {
            using var rng1 = new MultiThreadedRng(3, 6);
            using var rng2 = new MultiThreadedRng(3, 6);
            using var rng3 = new MultiThreadedRng(3, 7);
            using var rng4 = new MultiThreadedRng(6, 3);

            var rng1Sample = new double[10];
            for (var n = 0; n < rng1Sample.Length; n++)
                rng1Sample[n] = await rng1.GetUniform();

            var rng2Sample = new double[10];
            for (var n = 0; n < rng2Sample.Length; n++)
                rng2Sample[n] = await rng2.GetUniform();
            
            var rng3Sample = new double[10];
            for (var n = 0; n < rng3Sample.Length; n++)
                rng3Sample[n] = await rng3.GetUniform();
            
            var rng4Sample = new double[10];
            for (var n = 0; n < rng4Sample.Length; n++)
                rng4Sample[n] = await rng4.GetUniform();

            Assert.That(rng1Sample, Is.EquivalentTo(rng2Sample));
            Assert.That(rng1Sample, Is.Not.EquivalentTo(rng3Sample));
            Assert.That(rng1Sample, Is.Not.EquivalentTo(rng4Sample));
            Assert.That(rng2Sample, Is.Not.EquivalentTo(rng3Sample));
            Assert.That(rng2Sample, Is.Not.EquivalentTo(rng4Sample));
            Assert.That(rng3Sample, Is.Not.EquivalentTo(rng4Sample));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task NoSeed01()
        {
            using var rng1 = new MultiThreadedRng();
            using var rng2 = new MultiThreadedRng();
            using var rng3 = new MultiThreadedRng();

            var rng1Sample = new double[10];
            for (var n = 0; n < rng1Sample.Length; n++)
                rng1Sample[n] = await rng1.GetUniform();
            
            var rng2Sample = new double[10];
            for (var n = 0; n < rng2Sample.Length; n++)
                rng2Sample[n] = await rng2.GetUniform();
            
            var rng3Sample = new double[10];
            for (var n = 0; n < rng3Sample.Length; n++)
                rng3Sample[n] = await rng3.GetUniform();

            Assert.That(rng1Sample, Is.Not.EquivalentTo(rng2Sample));
            Assert.That(rng1Sample, Is.Not.EquivalentTo(rng3Sample));
            Assert.That(rng2Sample, Is.Not.EquivalentTo(rng3Sample));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestCancellation01()
        {
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            tokenSource.Cancel();
            
            using var rng2 = new MultiThreadedRng();
            var dist = new Uniform(rng2);
            Assert.That(await dist.NextNumber(1, 100_000, token), Is.EqualTo(0));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestCancellation02()
        {
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            tokenSource.Cancel();
            
            using var rng2 = new MultiThreadedRng();
            Assert.That(await rng2.GetUniform(token), Is.NaN);
        }
    }
}