using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using FastRng.Float;
using FastRng.Float.Distributions;
using NUnit.Framework;

namespace FastRngTests.Float
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
            for (var n = 0.0f; n < 1e6f; n++)
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
            Assert.That(await rng.NextNumber(5f, 6f, dist), Is.InRange(5f, 6f));
            Assert.That(await rng.NextNumber(0f, 1f, dist), Is.InRange(0f, 1f));
            Assert.That(await rng.NextNumber(3e9f, 3e9f+2f, dist), Is.InRange(3e9f, 3e9f+2f));
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
            Assert.That(await rng.NextNumber(10.0f, 1f, dist), Is.InRange(1f, 10f));
            Assert.That(await rng.NextNumber(20.0f, 1f, dist), Is.InRange(1f, 20f));
        }

        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TestStoppingProducers01()
        {
            var rng = new MultiThreadedRng();
            rng.StopProducer();

            var masterToken = new CancellationTokenSource(TimeSpan.FromSeconds(16)).Token;
            var wasCanceled = false;
            
            while(true)
            {
                var tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(3));
                await rng.GetUniform(tokenSource.Token);
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
            await rng.NextNumber(new NormalS02M05(), tokenSource2.Token);
            Assert.That(tokenSource2.IsCancellationRequested, Is.True);
            
            tokenSource2 = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            await rng.NextNumber(-1f, 1f, new NormalS02M05(), tokenSource2.Token);
            Assert.That(tokenSource2.IsCancellationRequested, Is.True);
            
            tokenSource2 = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            await rng.NextNumber(0u, 6u, new NormalS02M05(), tokenSource2.Token);
            Assert.That(tokenSource2.IsCancellationRequested, Is.True);
            
            tokenSource2 = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            await rng.NextNumber(0ul, 6ul, new NormalS02M05(), tokenSource2.Token);
            Assert.That(tokenSource2.IsCancellationRequested, Is.True);
        }

        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task OneSeed01()
        {
            var rng1 = new MultiThreadedRng(6);
            var rng2 = new MultiThreadedRng(6);
            var rng3 = new MultiThreadedRng(7);

            var rng1Sample = new float[10];
            for (var n = 0; n < rng1Sample.Length; n++)
                rng1Sample[n] = await rng1.GetUniform();
            
            var rng2Sample = new float[10];
            for (var n = 0; n < rng2Sample.Length; n++)
                rng2Sample[n] = await rng2.GetUniform();
            
            var rng3Sample = new float[10];
            for (var n = 0; n < rng3Sample.Length; n++)
                rng3Sample[n] = await rng3.GetUniform();
            
            rng1.StopProducer();
            rng2.StopProducer();
            rng3.StopProducer();
            
            Assert.That(rng1Sample, Is.EquivalentTo(rng2Sample));
            Assert.That(rng1Sample, Is.Not.EquivalentTo(rng3Sample));
            Assert.That(rng2Sample, Is.Not.EquivalentTo(rng3Sample));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task TwoSeeds01()
        {
            var rng1 = new MultiThreadedRng(3, 6);
            var rng2 = new MultiThreadedRng(3, 6);
            var rng3 = new MultiThreadedRng(3, 7);
            var rng4 = new MultiThreadedRng(6, 3);

            var rng1Sample = new float[10];
            for (var n = 0; n < rng1Sample.Length; n++)
                rng1Sample[n] = await rng1.GetUniform();

            var rng2Sample = new float[10];
            for (var n = 0; n < rng2Sample.Length; n++)
                rng2Sample[n] = await rng2.GetUniform();
            
            var rng3Sample = new float[10];
            for (var n = 0; n < rng3Sample.Length; n++)
                rng3Sample[n] = await rng3.GetUniform();
            
            var rng4Sample = new float[10];
            for (var n = 0; n < rng4Sample.Length; n++)
                rng4Sample[n] = await rng4.GetUniform();
            
            rng1.StopProducer();
            rng2.StopProducer();
            rng3.StopProducer();
            rng4.StopProducer();
            
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
            var rng1 = new MultiThreadedRng();
            var rng2 = new MultiThreadedRng();
            var rng3 = new MultiThreadedRng();

            var rng1Sample = new float[10];
            for (var n = 0; n < rng1Sample.Length; n++)
                rng1Sample[n] = await rng1.GetUniform();
            
            var rng2Sample = new float[10];
            for (var n = 0; n < rng2Sample.Length; n++)
                rng2Sample[n] = await rng2.GetUniform();
            
            var rng3Sample = new float[10];
            for (var n = 0; n < rng3Sample.Length; n++)
                rng3Sample[n] = await rng3.GetUniform();
            
            rng1.StopProducer();
            rng2.StopProducer();
            rng3.StopProducer();
            
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
            var dist = new Uniform();
            Assert.That(await rng2.NextNumber(1, 100_000, dist, token), Is.EqualTo(0));
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