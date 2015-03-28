using NUnit.Framework;

namespace Ra180.Tests
{
    [TestFixture]
    public class Ra180DtmTests
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
        public void ShouldDisplayTitle()
        {
            _ra180.SendKey(Ra180Key.DTM);
            Assert.That(_ra180.Display.ToString(), Is.StringContaining("(DTM)"));
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("        "));
        }
    }
}