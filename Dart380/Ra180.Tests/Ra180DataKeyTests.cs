using System.Linq;
using NUnit.Framework;

namespace Ra180.Tests
{
    [TestFixture]
    public class Ra180DataKeyTests
    {
        [Test]
        [TestCase("751", ExpectedResult = "7513")]
        [TestCase("442", ExpectedResult = "4422")]
        [TestCase("221", ExpectedResult = "2211")]
        [TestCase("330", ExpectedResult = "3300")]
        [TestCase("551", ExpectedResult = "5511")]
        [TestCase("432", ExpectedResult = "4325")]
        [TestCase("562", ExpectedResult = "5621")]
        [TestCase("320", ExpectedResult = "3201")]
        [TestCase("510", ExpectedResult = "5104")]
        [TestCase("350", ExpectedResult = "3506")]
        public string AddChecksum_ThreeDigits_ReturnsWithChecksum(string data)
        {
            return Ra180DataKey.AddChecksum(data);
        }

        [Test]
        [TestCase("7513", ExpectedResult = true)]
        [TestCase("4422", ExpectedResult = true)]
        [TestCase("2211", ExpectedResult = true)]
        [TestCase("3300", ExpectedResult = true)]
        [TestCase("5511", ExpectedResult = true)]
        [TestCase("4325", ExpectedResult = true)]
        [TestCase("5621", ExpectedResult = true)]
        [TestCase("3201", ExpectedResult = true)]
        [TestCase("5104", ExpectedResult = true)]
        [TestCase("3506", ExpectedResult = true)]
        [TestCase("7510", ExpectedResult = false)]
        [TestCase("4421", ExpectedResult = false)]
        [TestCase("2210", ExpectedResult = false)]
        public bool IsValid(string pn)
        {
            return Ra180DataKey.IsValid(pn);
        }

        [Test]
        public void CalculateChecksum_GivenEightGroupsWithFourChars_ReturnsThreeDigitChecksum()
        {
            var actual = Ra180DataKey.CalculateChecksum(new[] { "4422", "2211", "3300", "5511", "4325", "5621", "3201", "5104" });
            Assert.That(actual, Is.EqualTo("762"));
        }

        [Test]
        public void Constructor_GivenEightGroupsWithFourCharsEach_DataIsIdentical()
        {
            var expectedData = new[] {"4422", "2211", "3300", "5511", "4325", "5621", "3201", "5104"};
            var actual = new Ra180DataKey(expectedData);

            Assert.That(actual.Data, Is.EqualTo(expectedData), "#1");
            Assert.That(actual.Checksum, Is.EqualTo("762"), "#2");
        }

        [Test]
        public void Generate_ReturnsNewInstanceWithValidDataAndChecksum()
        {
            var actual = Ra180DataKey.Generate();

            foreach (var pn in actual.Data)
                Assert.That(Ra180DataKey.IsValid(pn), "#1 " + pn);
            
            Assert.That(actual.Checksum, Is.EqualTo(Ra180DataKey.CalculateChecksum(actual.Data)), "#2");
        }

        [Test]
        public void Generate_ReturnsWithEightGroupsAndSizeOfThree()
        {
            var actual = Ra180DataKey.Generate();

            Assert.That(actual.Data, Has.Length.EqualTo(8), "#1 Data.Length");
            Assert.That(actual.Checksum, Has.Length.EqualTo(3), "#2 Checksum.Length");
            Assert.That(actual.Data.Min(s => s.Length), Is.EqualTo(4), "#3 Data.Min(Length)");
            Assert.That(actual.Data.Max(s => s.Length), Is.EqualTo(4), "#4 Data.Max(Length)");
        }
    }
}