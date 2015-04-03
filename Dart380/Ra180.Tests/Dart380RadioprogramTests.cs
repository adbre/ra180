using NUnit.Framework;
using Ra180.Devices.Dart380;

namespace Ra180.Tests
{
    [TestFixture]
    public class Dart380RadioprogramTests
    {
        private Dart380 _dart;
        private Ra180 _ra180;
        private DelayedSynchronizationContext _synchronizationContext;

        [SetUp]
        public void SetUp()
        {
            _synchronizationContext = new DelayedSynchronizationContext();
            _ra180 = new Ra180(new EmptyRa180Network(), _synchronizationContext);
            _dart = new Dart380(_synchronizationContext) { Ra180 = _ra180 };

            _dart.SendKey(Ra180Key.ModSKYDD);
            _ra180.SendKey(Ra180Key.ModSKYDD);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
        }

        [Test]
        public void Tid()
        {
            _dart.SendKey(Ra180Key.TID);
            Assert.That(_dart.SmallDisplay.ToString(), Is.StringMatching("^T:"));
            _dart.SendKey(Ra180Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.StringMatching("^DAT:"));
            _dart.SendKey(Ra180Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.StringContaining("(TID)"));
            _dart.SendKey(Ra180Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("        "));
        }

        [Test]
        public void Rda()
        {
            _dart.SendKey(Ra180Key.RDA);
            Assert.That(_dart.SmallDisplay.ToString(), Is.StringMatching("^SDX"));
            _dart.SendKey(Ra180Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.StringMatching("^BAT="));
            _dart.SendKey(Ra180Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.StringContaining("(RDA)"));
            _dart.SendKey(Ra180Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("        "));
        }

        [Test]
        public void Kda()
        {
            _dart.SendKey(Ra180Key.KDA);
            Assert.That(_dart.SmallDisplay.ToString(), Is.StringMatching("^FR:"));
            _dart.SendKey(Ra180Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.StringMatching(@"^BD1:"));
            _dart.SendKey(Ra180Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.StringMatching(@"^BD2:"));
            _dart.SendKey(Ra180Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.StringMatching(@"^SYNK=NEJ"));
            _dart.SendKey(Ra180Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.StringMatching(@"^PNY:###"));
            _dart.SendKey(Ra180Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.StringContaining("(KDA)"));
            _dart.SendKey(Ra180Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("        "));
        }
    }
}