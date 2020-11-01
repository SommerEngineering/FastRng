using System;
using System.Diagnostics.CodeAnalysis;
using FastRng.Float;
using NUnit.Framework;

namespace FastRngTests.Float
{
    [ExcludeFromCodeCoverage]
    public class MathToolsTests
    {
        #region Gamma

        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void GammaTest01()
        {
            Assert.That(MathTools.Gamma(-0.5f), Is.EqualTo(-3.544907701811087f).Within(1e-6f));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void GammaTest02()
        {
            Assert.That(MathTools.Gamma(0.1f), Is.EqualTo(9.51350975f).Within(1e-6f));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void GammaTest03()
        {
            Assert.That(MathTools.Gamma(0.5f), Is.EqualTo(1.772453850905517f).Within(1e-6f));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void GammaTest04()
        {
            Assert.That(MathTools.Gamma(1.0f), Is.EqualTo(1.0f).Within(1e-6f));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void GammaTest05()
        {
            Assert.That(MathTools.Gamma(1.5f), Is.EqualTo(0.8862269254527587f).Within(1e-6f));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void GammaTest06()
        {
            Assert.That(MathTools.Gamma(2.0f), Is.EqualTo(1.0f).Within(1e-6f));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void GammaTest07()
        {
            Assert.That(MathTools.Gamma(3.0f), Is.EqualTo(2.0f).Within(1e-6f));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void GammaTest08()
        {
            Assert.That(MathTools.Gamma(10.0f), Is.EqualTo(362_880.719f).Within(1e-6f));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void GammaTest09()
        {
            Assert.That(MathTools.Gamma(140.0f), Is.EqualTo(float.NaN));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void GammaTest10()
        {
            Assert.That(MathTools.Gamma(170.0f), Is.EqualTo(float.NaN));
        }

        #endregion

        #region Factorial (integer)

        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void FactorialInteger01()
        {
            Assert.That(MathTools.Factorial(0), Is.EqualTo(1));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void FactorialInteger02()
        {
            Assert.That(MathTools.Factorial(1), Is.EqualTo(1));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void FactorialInteger03()
        {
            Assert.That(MathTools.Factorial(2), Is.EqualTo(2));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void FactorialInteger04()
        {
            Assert.That(MathTools.Factorial(3), Is.EqualTo(6));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void FactorialInteger05()
        {
            Assert.That(MathTools.Factorial(4), Is.EqualTo(24));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void FactorialInteger06()
        {
            Assert.That(MathTools.Factorial(5), Is.EqualTo(120));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void FactorialInteger07()
        {
            Assert.That(MathTools.Factorial(6), Is.EqualTo(720));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void FactorialInteger08()
        {
            Assert.That(MathTools.Factorial(7), Is.EqualTo(5_040));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void FactorialInteger09()
        {
            Assert.That(MathTools.Factorial(8), Is.EqualTo(40_320));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void FactorialInteger10()
        {
            Assert.That(MathTools.Factorial(9), Is.EqualTo(362_880));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void FactorialInteger11()
        {
            Assert.That(MathTools.Factorial(10), Is.EqualTo(3_628_800));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void FactorialInteger12()
        {
            Assert.That(MathTools.Factorial(11), Is.EqualTo(39_916_800));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void FactorialInteger13()
        {
            Assert.That(MathTools.Factorial(12), Is.EqualTo(479_001_600));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void FactorialInteger14()
        {
            Assert.That(MathTools.Factorial(13), Is.EqualTo(6_227_020_800));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void FactorialInteger15()
        {
            Assert.That(MathTools.Factorial(14), Is.EqualTo(87_178_291_200));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void FactorialInteger16()
        {
            Assert.That(MathTools.Factorial(15), Is.EqualTo(1_307_674_368_000));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void FactorialInteger17()
        {
            Assert.That(MathTools.Factorial(16), Is.EqualTo(20_922_789_888_000));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void FactorialInteger18()
        {
            Assert.That(MathTools.Factorial(17), Is.EqualTo(355_687_428_096_000));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void FactorialInteger19()
        {
            Assert.That(MathTools.Factorial(18), Is.EqualTo(6_402_373_705_728_000));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void FactorialInteger20()
        {
            Assert.That(MathTools.Factorial(19), Is.EqualTo(121_645_100_408_832_000));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void FactorialInteger21()
        {
            Assert.That(MathTools.Factorial(20), Is.EqualTo(2_432_902_008_176_640_000));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void FactorialInteger22()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => MathTools.Factorial(21));

            // Note: 21! is not possible in C# until we got 128 bit integers, since:
            //       ulong.max == 18_446_744_073_709_551_615 < 51_090_942_171_709_400_000
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void FactorialInteger23()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => MathTools.Factorial(45_646));

            // Note: 45_646! is not possible in C# since:
            //       ulong.max == 18_446_744_073_709_551_615
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void FactorialInteger24()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => MathTools.Factorial(-1));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void FactorialInteger25()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => MathTools.Factorial(-6_565));
        }

        #endregion

        #region Factorial (floating point)

        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void FactorialFloatingPoint01()
        {
            Assert.That(MathTools.Factorial(0.5f), Is.EqualTo(0.886226925f).Within(1e6f));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void FactorialFloatingPoint02()
        {
            Assert.That(MathTools.Factorial(1.5f), Is.EqualTo(1.329340388f).Within(1e6f));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void FactorialFloatingPoint03()
        {
            Assert.That(MathTools.Factorial(-1.5f), Is.EqualTo(-1.329340388f).Within(1e6f));
        }
        
        [Test]
        [Category(TestCategories.COVER)]
        [Category(TestCategories.NORMAL)]
        public void FactorialFloatingPoint04()
        {
            Assert.That(MathTools.Factorial(7.5f), Is.EqualTo(14_034.407293483f).Within(1e6f));
        }

        #endregion
    }
}