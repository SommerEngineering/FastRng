using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using FastRng.Float;
using FastRng.Float.Distributions;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.Random;
using NUnit.Framework;

namespace FastRngTests.Float
{
    [ExcludeFromCodeCoverage]
    public class PerformanceTests
    {
        #region FastRng

        [Test]
        [Category(TestCategories.PERFORMANCE)]
        public async Task Generate1MUniform()
        {
            using var rng = new MultiThreadedRng();
            var data = new float[1_000_000];
            var stopwatch = new Stopwatch();
            Thread.Sleep(TimeSpan.FromSeconds(10)); // Warm-up phase of generator
            
            stopwatch.Start();
            for (uint n = 0; n < data.Length; n++)
                data[n] = await rng.GetUniform();
            
            stopwatch.Stop();

            TestContext.WriteLine($"Generated 1M uniform distributed random numbers in {stopwatch.Elapsed.Minutes} minute(s), {stopwatch.Elapsed.Seconds} second(s), and {stopwatch.Elapsed.Milliseconds} milliseconds.");
        }
        
        [Test]
        [Category(TestCategories.PERFORMANCE)]
        public async Task Generate1MNormal()
        {
            using var rng = new MultiThreadedRng();
            var dist = new NormalS02M05(rng);
            var data = new float[1_000_000];
            var stopwatch = new Stopwatch();
            Thread.Sleep(TimeSpan.FromSeconds(10)); // Warm-up phase of generator
            
            stopwatch.Start();
            for (uint n = 0; n < data.Length; n++)
                data[n] = await dist.NextNumber();
            
            stopwatch.Stop();

            TestContext.WriteLine($"Generated 1M normal distributed random numbers in {stopwatch.Elapsed.Minutes} minute(s), {stopwatch.Elapsed.Seconds} second(s), and {stopwatch.Elapsed.Milliseconds} milliseconds.");
        }
        
        [Test]
        [Category(TestCategories.PERFORMANCE)]
        public async Task Generate1MChiSquare()
        {
            using var rng = new MultiThreadedRng();
            var dist = new ChiSquareK4(rng);
            var data = new float[1_000_000];
            var stopwatch = new Stopwatch();
            Thread.Sleep(TimeSpan.FromSeconds(10)); // Warm-up phase of generator
            
            stopwatch.Start();
            for (uint n = 0; n < data.Length; n++)
                data[n] = await dist.NextNumber();
            
            stopwatch.Stop();

            TestContext.WriteLine($"Generated 1M chi-square distributed random numbers in {stopwatch.Elapsed.Minutes} minute(s), {stopwatch.Elapsed.Seconds} second(s), and {stopwatch.Elapsed.Milliseconds} milliseconds.");
        }

        #endregion

        #region Math.NET

        [Test]
        [Category(TestCategories.PERFORMANCE)]
        public void ComparisonMathNet1MUniform()
        {
            var rng = new Xorshift(true);
            var data = new float[1_000_000];
            var stopwatch = new Stopwatch();
            Thread.Sleep(TimeSpan.FromSeconds(10)); // Warm-up phase of generator
            
            stopwatch.Start();
            for (uint n = 0; n < data.Length; n++)
                data[n] = (float) rng.NextDouble();
            
            stopwatch.Stop();
            
            TestContext.WriteLine($"Generated 1M uniform distributed random numbers by means of Math.NET in {stopwatch.Elapsed.Minutes} minute(s), {stopwatch.Elapsed.Seconds} second(s), and {stopwatch.Elapsed.Milliseconds} milliseconds.");
        }
        
        [Test]
        [Category(TestCategories.PERFORMANCE)]
        public void ComparisonMathNet1MNormal()
        {
            var rng = new Xorshift(true);
            var dist = new Normal(stddev: 0.2f, mean: 0.5f, randomSource: rng);
            var data = new float[1_000_000];
            var stopwatch = new Stopwatch();
            Thread.Sleep(TimeSpan.FromSeconds(10)); // Warm-up phase of generator
            
            stopwatch.Start();
            for (uint n = 0; n < data.Length; n++)
                data[n] = (float) dist.Sample();
            
            stopwatch.Stop();
            
            TestContext.WriteLine($"Generated 1M normal distributed random numbers by means of Math.NET in {stopwatch.Elapsed.Minutes} minute(s), {stopwatch.Elapsed.Seconds} second(s), and {stopwatch.Elapsed.Milliseconds} milliseconds.");
        }
        
        [Test]
        [Category(TestCategories.PERFORMANCE)]
        public void ComparisonMathNet1MChiSquare()
        {
            var rng = new Xorshift(true);
            var dist = new ChiSquared(4);
            var data = new float[1_000_000];
            var stopwatch = new Stopwatch();
            Thread.Sleep(TimeSpan.FromSeconds(10)); // Warm-up phase of generator
            
            stopwatch.Start();
            for (uint n = 0; n < data.Length; n++)
                data[n] = (float) dist.Sample();
            
            stopwatch.Stop();
            
            TestContext.WriteLine($"Generated 1M chi-squared distributed random numbers by means of Math.NET in {stopwatch.Elapsed.Minutes} minute(s), {stopwatch.Elapsed.Seconds} second(s), and {stopwatch.Elapsed.Milliseconds} milliseconds.");
        }

        #endregion
    }
}