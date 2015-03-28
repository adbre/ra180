using NUnit.Framework;

namespace Ra180.Tests
{
    [TestFixture]
    public class Dart380FjärrTests
    {
        private Dart380 _dart;
        private Ra180 _ra180;
        private DelayedSynchronizationContext _synchronizationContext;

        [SetUp]
        public void SetUp()
        {
            _synchronizationContext = new DelayedSynchronizationContext();
            _ra180 = new Ra180(new EmptyRa180Network(), _synchronizationContext);
            _dart = new Dart380(_synchronizationContext) {Ra180 = _ra180};

            _dart.SendKey(Ra180Key.ModKLAR);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
        }

        [Test]
        [TestCase(Ra180Key.TID)]
        [TestCase(Ra180Key.RDA)]
        [TestCase(Ra180Key.KDA)]
        [TestCase(Ra180Key.NYK)]
        public void Tid_Ra180NotStarted_DisplaysEjFjärr(string key)
        {
            _dart.SendKey(key);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("EJ FJÄRR"));
        }
    }
}