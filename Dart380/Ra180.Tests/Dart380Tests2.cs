using Moq;
using NUnit.Framework;
using Ra180.Devices.Dart380;

namespace Ra180.Tests
{
    [TestFixture]
    public class Dart380Tests2
    {
        private Dart380 _dart;
        private Ra180 _ra180;
        private DelayedSynchronizationContext _synchronizationContext;

        [SetUp]
        public void SetUp()
        {
            _synchronizationContext = new DelayedSynchronizationContext();
            _ra180 = new Ra180(new EmptyRadio(), _synchronizationContext);
            _dart = new Dart380(_synchronizationContext) { Ra180 = _ra180 };

            _dart.SendKey(Ra180Key.ModSKYDD);
            _ra180.SendKey(Dart380Key.ModSKYDD);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);

            Dart380Helper.SetUp(_dart);
        }

        [Test]
        public void KanSändTextmeddelande()
        {
            _dart.SendKey(Dart380Key.FMT);
            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("FORMAT:         "));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("ROT*NIVÅ"));
            _dart.SendKey("1");
            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("FORMAT:1        "));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("LEDNING "));
            _dart.SendKey("0");
            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("FORMAT:10       "));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("LEDNSBTJ"));
            _dart.SendKey("0");
            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("FORMAT:100      "));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("FRI*TEXT"));
            _dart.SendKey(Dart380Key.ENT);
            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("FRI*TEXT*       "));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("FRI*TEXT"));
            _dart.SendKey(Dart380Key.ENT);
            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("TILL:           "));
            _dart.SendKey(Dart380Key.ÄND);
            _dart.SendKeys("JA");
            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("TILL:JA         "));
            _dart.SendKey(Dart380Key.ENT);
            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("                "));
            _dart.SendKey(Dart380Key.ENT);
            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("291504*FR:CR    "));
            _dart.SendKey(Dart380Key.ENT);
            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("                "));
            _dart.SendKey(Dart380Key.ENT);
            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("FRÅN:     *U:   "));
            _dart.SendKey(Dart380Key.ENT);
            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("TEXT:           "));
            _dart.SendKeys("FÖRBERED RÖ");
            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("TEXT:FÖRBERED RÖ"));
            _dart.SendKeys("K 10 LAG");
            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("K 10 LAG        "));
            _dart.SendKey(Dart380Key.SLT);
            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("K 10 LAG        "));
            _dart.SendKey(Dart380Key.SLT);
            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("                "));
        }

        [Test]
        public void Isk_BörjarPåTnrOchAvsRad()
        {
            
        }
    }
}