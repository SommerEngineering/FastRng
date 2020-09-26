using System;
using System.Threading.Tasks;
using FastRng.Double;
using NUnit.Framework;

namespace FastRngTests.Double.Distributions
{
    public class Uniform
    {
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task KolmogorovSmirnovTest()
        {
            // Kolmogorov-Smirnov test for distributions.
            // See Knuth volume 2, page 48-51 (third edition).
            // This test should *fail* on average one time in 1000 runs.
            // That's life with random number generators: if the test passed all the time, 
            // the source wouldn't be random enough!  If the test were to fail more frequently,
            // the most likely explanation would be a bug in the code.
            
            const int NUM_ROUNDS = 10_000;
            const double FAILURE_PROBABILITY = 0.001; // probability of test failing with normal distributed input
            const double P_LOW = 0.25 * FAILURE_PROBABILITY;
            const double P_HIGH = 1.0 - 0.25 * FAILURE_PROBABILITY;
            
            var samples = new double[NUM_ROUNDS];
            var rng = new MultiThreadedRng();
            int n;
             
            for (n = 0; n != NUM_ROUNDS; ++n)
                samples[n] = await rng.GetUniform();
 
            rng.StopProducer();
            Array.Sort(samples);

            var jMinus = 0;
            var jPlus = 0;
            var kPlus = -double.MaxValue;
            var kMinus = -double.MaxValue;
 
            for (n = 0; n != NUM_ROUNDS; ++n)
            {
                var cdf = samples[n];
                var temp = (n + 1.0) / NUM_ROUNDS - cdf;
                
                if (kPlus < temp)
                {
                    kPlus = temp;
                    jPlus = n;
                }
                
                temp = cdf - (n + 0.0) / NUM_ROUNDS;
                if (kMinus < temp)
                {
                    kMinus = temp;
                    jMinus = n;
                }
            }
 
            var sqrtNumReps = Math.Sqrt(NUM_ROUNDS);
            kPlus *= sqrtNumReps;
            kMinus *= sqrtNumReps;
 
            // We divide the failure probability by four because we have four tests:
            // left and right tests for K+ and K-.
            var cutoffLow = Math.Sqrt(0.5 * Math.Log(1.0 / (1.0 - P_LOW))) - 1.0 / (6.0 * sqrtNumReps);
            var cutoffHigh = Math.Sqrt(0.5 * Math.Log(1.0 / (1.0 - P_HIGH))) - 1.0 / (6.0 * sqrtNumReps);
 
            TestContext.WriteLine($"K+ = {kPlus} | K- = {kMinus}");
            TestContext.WriteLine($"K+ max at position {jPlus} = {samples[jPlus]}");
            TestContext.WriteLine($"K- max at position {jMinus} = {samples[jMinus]}");
            TestContext.WriteLine($"Acceptable interval: [{cutoffLow}, {cutoffHigh}]");

            Assert.That(kPlus, Is.GreaterThanOrEqualTo(cutoffLow), "K+ is lower than low cutoff");
            Assert.That(kPlus, Is.LessThanOrEqualTo(cutoffHigh), "K+ is higher than high cutoff");
            Assert.That(kMinus, Is.GreaterThanOrEqualTo(cutoffLow), "K- is lower than low cutoff");
            Assert.That(kMinus, Is.LessThanOrEqualTo(cutoffHigh), "K- is lower than high cutoff");
        }
    }
}