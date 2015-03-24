using NUnit.Framework;

namespace Ra180.Tests
{
    [TestFixture]
    public class Ra180RdaTests
    {
        private Ra180 _ra180;
        private DelayedSynchronizationContext _synchronizationContext;

        [SetUp]
        public void SetUp()
        {
            _synchronizationContext = new DelayedSynchronizationContext();
            _ra180 = new Ra180(new EmptyRa180Network(), _synchronizationContext);

            _ra180.SendKey(Ra180Key.ModKLAR);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
        }

        [Test]
        public void ShouldNavigateRDA()
        {
            _ra180.SendKey(Ra180Key.RDA);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("SDX=NEJ "));
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("OPMTN=JA"));
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("BAT=13.1"));
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.StringContaining("(RDA)"));
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("        "));
        }

        [Test]
        public void ShouldReturnToMainMenuFromRDAOnSLT()
        {
            _ra180.SendKey(Ra180Key.RDA);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("SDX=NEJ "));
            _ra180.SendKey(Ra180Key.SLT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("        "));

            _ra180.SendKey(Ra180Key.RDA);
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("OPMTN=JA"));
            _ra180.SendKey(Ra180Key.SLT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("        "));

            _ra180.SendKey(Ra180Key.RDA);
            _ra180.SendKey(Ra180Key.ENT);
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("BAT=13.1"));
            _ra180.SendKey(Ra180Key.SLT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("        "));

            _ra180.SendKey(Ra180Key.RDA);
            _ra180.SendKey(Ra180Key.ENT);
            _ra180.SendKey(Ra180Key.ENT);
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.StringContaining("(RDA)"));
            _ra180.SendKey(Ra180Key.SLT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("        "));
        }
    }
}