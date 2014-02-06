using C42A.Ra180.Infrastructure;
using NUnit.Framework;

namespace C42A.Ra180.Tests
{
    [TestFixture]
    public class PassiveKeyCalculatorTests
    {
        [Test]
        [TestCase("751", "7513")]
        [TestCase("442", "4422")]
        [TestCase("221", "2211")]
        [TestCase("330", "3300")]
        [TestCase("551", "5511")]
        [TestCase("432", "4325")]
        [TestCase("562", "5621")]
        [TestCase("320", "3201")]
        [TestCase("510", "5104")]
        public void ShouldCalculatePnyGroup(string input, string expected)
        {
            var sut = new PassiveKeyCalculator();
            var actual = sut.GetPnyGroup(input);
            Assert.That(actual, Is.EqualTo(expected), "#1");
        }

        [Test]
        [TestCase("4422 2211 3300 5511 4325 5621 3201 5104", "762")]
        [TestCase("7513 4422 2211 3300 5511 4325 5621 3201 5104", "033")]
        public void ShouldCalculateNyk(string input, string expected)
        {
            var sut = new PassiveKeyCalculator();
            var actual = sut.GetNyk(input);
            Assert.That(actual, Is.EqualTo(expected), "#1");
        }

        [Test]
        public void ShouldGenerateValidKey()
        {
            var sut = new PassiveKeyCalculator();
            var key = sut.GenerateNewKey();

            var groups = key.ToArray();
            var nyk = sut.GetNyk(groups);
            Assert.That(key.NYK, Is.EqualTo(nyk), "#1 NYK");

            for (var i = 0; i < groups.Length; i++)
            {
                var pn = groups[i];
                var threeLetters = pn.Substring(0, 3);
                var check = sut.GetPnyGroup(threeLetters);
                Assert.That(pn, Is.EqualTo(check), string.Format("#2 PN{0}", i + 1));
            }
        }
    }
}