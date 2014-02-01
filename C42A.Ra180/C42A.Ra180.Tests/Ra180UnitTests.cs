using C42A.Ra180.Infrastructure;
using NUnit.Framework;

namespace C42A.Ra180.Tests
{
    [TestFixture]
    public class Ra180UnitTests
    {
        [SetUp]
        public void SetUp()
        {
            
        }

        private Ra180Unit GetSystemUnderTest()
        {
            return new Ra180Unit();
        }

        [Test]
        public void ShouldBeFrånByDefault()
        {
            var sut = GetSystemUnderTest();
            var actual = sut.Mod;

            Assert.That(actual, Is.EqualTo(Ra180Mod.Från), "#1");
        }

        [Test]
        public void ShouldBeKanal1ByDefault()
        {
            var sut = GetSystemUnderTest();
            var actual = sut.Kanal;

            Assert.That(actual, Is.EqualTo(1), "#1");
        }

        [Test]
        public void ShouldBeVolym4ByDefault()
        {
            var sut = GetSystemUnderTest();
            Assert.That(sut.Volym, Is.EqualTo(4), "#1");
        }

        [Test]
        [TestCase(Ra180Knapp.Från, Ra180Mod.Från)]
        [TestCase(Ra180Knapp.Klar, Ra180Mod.Klar)]
        [TestCase(Ra180Knapp.Skydd, Ra180Mod.Skydd)]
        [TestCase(Ra180Knapp.DRelä, Ra180Mod.DRelä)]
        public void ShouldSetModToKlarOnKeyKlar(Ra180Knapp knapp, Ra180Mod expected)
        {
            var sut = GetSystemUnderTest();
            sut.SendKeys(knapp);

            Assert.That(sut.Mod, Is.EqualTo(expected), "#1");
        }

        [Test]
        [TestCase(Ra180Knapp.Kanal1, 1)]
        [TestCase(Ra180Knapp.Kanal2, 2)]
        [TestCase(Ra180Knapp.Kanal3, 3)]
        [TestCase(Ra180Knapp.Kanal4, 4)]
        [TestCase(Ra180Knapp.Kanal5, 5)]
        [TestCase(Ra180Knapp.Kanal6, 6)]
        [TestCase(Ra180Knapp.Kanal7, 7)]
        [TestCase(Ra180Knapp.Kanal8, 8)]
        public void ShouldChangeKanal(Ra180Knapp knapp, int expected)
        {
            var sut = GetSystemUnderTest();
            sut.SendKeys(knapp);

            Assert.That(sut.Kanal, Is.EqualTo(expected), "#1");
        }

        [Test]
        [TestCase(Ra180Knapp.Volym1, 1)]
        [TestCase(Ra180Knapp.Volym2, 2)]
        [TestCase(Ra180Knapp.Volym3, 3)]
        [TestCase(Ra180Knapp.Volym4, 4)]
        [TestCase(Ra180Knapp.Volym5, 5)]
        [TestCase(Ra180Knapp.Volym6, 6)]
        [TestCase(Ra180Knapp.Volym7, 7)]
        [TestCase(Ra180Knapp.Volym8, 8)]
        public void ShouldChangeVolym(Ra180Knapp knapp, int expected)
        {
            var sut = GetSystemUnderTest();
            sut.SendKeys(knapp);

            Assert.That(sut.Volym, Is.EqualTo(expected), "#1");
        }

        [Test]
        public void ShouldDisplayTime()
        {
            var sut = GetSystemUnderTest();
            sut.SendKeys(Ra180Knapp.Klar);
            sut.SendKeys(Ra180Knapp.Knapp1);

            Assert.That(sut.Display, Is.EqualTo("T:000000"), "#1");
        }

        [Test]
        public void ShouldNotDisplayTimeWhenFrån()
        {
            var sut = GetSystemUnderTest();
            sut.SendKeys(Ra180Knapp.Knapp1);

            Assert.That(sut.Display, Is.EqualTo(null), "#1");
        }

        [Test]
        public void ShouldSetDateAndTime()
        {
            var sut = GetSystemUnderTest();
            sut.SendKeys(Ra180Knapp.Klar);
            sut.SendKeys(Ra180Knapp.Knapp1);
            sut.SendKeys(Ra180Knapp.ÄND);
            Assert.That(sut.Display, Is.EqualTo("T:"), "#1");

            sut.SendKeys(Ra180Knapp.Knapp2);
            sut.SendKeys(Ra180Knapp.Knapp1);
            sut.SendKeys(Ra180Knapp.Knapp5);
            sut.SendKeys(Ra180Knapp.Knapp0);
            sut.SendKeys(Ra180Knapp.Knapp3);
            sut.SendKeys(Ra180Knapp.Knapp6);
            Assert.That(sut.Display, Is.EqualTo("T:215036"), "#2");

            sut.SendKeys(Ra180Knapp.RETUR);
            Assert.That(sut.Display, Is.EqualTo("T:215036"), "#3");

            sut.SendKeys(Ra180Knapp.RETUR);
            Assert.That(sut.Display, Is.EqualTo("DAT:0101"), "#4");
            sut.SendKeys(Ra180Knapp.ÄND);
            Assert.That(sut.Display, Is.EqualTo("DAT:"), "#5");
            sut.SendKeys(Ra180Knapp.Knapp0);
            sut.SendKeys(Ra180Knapp.Knapp1);
            sut.SendKeys(Ra180Knapp.Knapp2);
            sut.SendKeys(Ra180Knapp.Knapp9);
            Assert.That(sut.Display, Is.EqualTo("DAT:0129"), "#6");

            sut.SendKeys(Ra180Knapp.RETUR);
            Assert.That(sut.Display, Is.EqualTo("DAT:0129"), "#7");

            sut.SendKeys(Ra180Knapp.RETUR);
            Assert.That(sut.Display, Is.EqualTo("  (TID) "), "#8");

            sut.SendKeys(Ra180Knapp.RETUR);
            Assert.That(sut.Display, Is.EqualTo(null), "#9");

            sut.SendKeys(Ra180Knapp.Knapp1);
            Assert.That(sut.Display, Is.EqualTo("T:215036"), "#10");
            sut.SendKeys(Ra180Knapp.RETUR);
            Assert.That(sut.Display, Is.EqualTo("DAT:0129"), "#11");
            sut.SendKeys(Ra180Knapp.SLT);
            Assert.That(sut.Display, Is.EqualTo(null), "#12");
        }


        [Test]
        public void ShouldAbortTidInput()
        {
            var sut = GetSystemUnderTest();
            sut.SendKeys(Ra180Knapp.Klar);
            sut.SendKeys(Ra180Knapp.Knapp1);
            sut.SendKeys(Ra180Knapp.ÄND);
            Assert.That(sut.Display, Is.EqualTo("T:"), "#1");

            sut.SendKeys(Ra180Knapp.Knapp2);
            sut.SendKeys(Ra180Knapp.Knapp1);
            sut.SendKeys(Ra180Knapp.Knapp5);
            sut.SendKeys(Ra180Knapp.SLT);
            Assert.That(sut.Display, Is.EqualTo("T:000000"), "#2");

            sut.SendKeys(Ra180Knapp.SLT);
            Assert.That(sut.Display, Is.EqualTo(null), "#3");
        }
    }
}
