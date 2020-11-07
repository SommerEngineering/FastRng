using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FastRng.Double;
using NUnit.Framework;
using Uniform = FastRng.Double.Distributions.Uniform;

namespace FastRngTests.Double
{
    [ExcludeFromCodeCoverage]
    public class DecisionTester
    {
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task DecisionUniform01()
        {
            using var rng = new MultiThreadedRng();
            var dist = new Uniform(rng);

            var neededCoinTossesA = 0;
            var neededCoinTossesB = 0;
            var neededCoinTossesC = 0;

            for(var n = 0; n < 100; n++) while (!await dist.HasDecisionBeenMade(0.0f, 0.1f)) neededCoinTossesA++;
            for(var n = 0; n < 100; n++) while (!await dist.HasDecisionBeenMade(0.5f, 0.6f)) neededCoinTossesB++;
            for(var n = 0; n < 100; n++) while (!await dist.HasDecisionBeenMade(0.8f, 0.9f)) neededCoinTossesC++;

            var values = new[] {neededCoinTossesA, neededCoinTossesB, neededCoinTossesC};
            var max = values.Max();
            var min = values.Min();
            
            TestContext.WriteLine($"Coin tosses: a={neededCoinTossesA}, b={neededCoinTossesB}, c={neededCoinTossesC}");
            Assert.That(max - min, Is.LessThanOrEqualTo(250));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public async Task DecisionWeibull01()
        {
            using var rng = new MultiThreadedRng();
            var dist = new FastRng.Double.Distributions.WeibullK05La1(rng);

            var neededCoinTossesA = 0;
            var neededCoinTossesB = 0;
            var neededCoinTossesC = 0;

            for(var n = 0; n < 100; n++) while (!await dist.HasDecisionBeenMade(0.0f, 0.1f)) neededCoinTossesA++;
            for(var n = 0; n < 100; n++) while (!await dist.HasDecisionBeenMade(0.5f, 0.6f)) neededCoinTossesB++;
            for(var n = 0; n < 100; n++) while (!await dist.HasDecisionBeenMade(0.8f, 0.9f)) neededCoinTossesC++;

            var values = new[] {neededCoinTossesA, neededCoinTossesB, neededCoinTossesC};
            var max = values.Max();
            var min = values.Min();
            
            TestContext.WriteLine($"Coin tosses: a={neededCoinTossesA}, b={neededCoinTossesB}, c={neededCoinTossesC}");
            Assert.That(max - min, Is.LessThanOrEqualTo(2_800));
        }
    }
}