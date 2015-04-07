using NUnit.Framework;
using Ra180.Devices.Dart380;

namespace Ra180.Tests
{
    [TestFixture]
    public class IskTests
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
        public void Isk_BörjarPåTnrOchAvsRad()
        {
            Dart380Helper.WriteFmt100Msg(_dart);

            _dart.SendKey(Dart380Key.ISK);

            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("291504*FR:CR    "));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("FRI*TEXT"));
        }

        [Test]
        public void Isk_ExcessivePgUp_ShowsMessage()
        {
            Dart380Helper.WriteFmt100Msg(_dart);

            _dart.SendKey(Dart380Key.ISK);
            _dart.SendKey(Dart380Key.PGUP);
            _dart.SendKey(Dart380Key.PGUP);
            _dart.SendKey(Dart380Key.PGUP);

            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("291504*FR:CR    "));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("FRI*TEXT"));
        }

        [Test]
        public void Isk_NoMessage_ShowsTitle()
        {
            _dart.SendKey(Dart380Key.ISK);
            
            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("  (INSKRIVNA)   "));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("        "));
        }

        [Test]
        public void Isk_ExcessivePgDown_ShowsTitle()
        {
            _dart.SendKey(Dart380Key.ISK);
            _dart.SendKey(Dart380Key.PGDOWN);
            _dart.SendKey(Dart380Key.PGDOWN);
            _dart.SendKey(Dart380Key.PGDOWN);

            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("  (INSKRIVNA)   "));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("        "));
        }

        [Test]
        public void Isk_OneMessageAndMoveDown_ShowsTitle()
        {
            Dart380Helper.WriteFmt100Msg(_dart);

            _dart.SendKey(Dart380Key.ISK);
            _dart.SendKey(Dart380Key.PGDOWN);

            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("  (INSKRIVNA)   "));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("        "));
        }

        [Test]
        public void Isk_OneMessageAndExcessiveMove_IgnoresExtraPgDown()
        {
            Dart380Helper.WriteFmt100Msg(_dart);

            _dart.SendKey(Dart380Key.ISK);
            _dart.SendKey(Dart380Key.PGDOWN);
            _dart.SendKey(Dart380Key.PGDOWN);
            _dart.SendKey(Dart380Key.PGDOWN);
            _dart.SendKey(Dart380Key.PGDOWN);
            _dart.SendKey(Dart380Key.PGUP);

            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("291504*FR:CR    "));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("FRI*TEXT"));
        }

        [Test]
        public void Isk_OneMessageAndMoveDownThenUp_ShowsMessage()
        {
            Dart380Helper.WriteFmt100Msg(_dart);

            _dart.SendKey(Dart380Key.ISK);
            _dart.SendKey(Dart380Key.PGDOWN);
            _dart.SendKey(Dart380Key.PGUP);

            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("291504*FR:CR    "));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("FRI*TEXT"));
        }

        [Test]
        public void Isk_CanDeleteMessage()
        {
            Dart380Helper.WriteFmt100Msg(_dart);

            _dart.SendKey(Dart380Key.ISK);
            _dart.SendKey(Dart380Key.RAD);
            Assert.That(_dart.LargeDisplay.ToString(), Is.StringContaining("RADERA?"));
            _dart.SendKey(Dart380Key.RAD);
            Assert.That(_dart.LargeDisplay.ToString(), Is.StringContaining("RADERAD"));
            _dart.SendKey(Dart380Key.PGUP);

            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("  (INSKRIVNA)   "));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("        "));
        }

        [Test]
        public void Isk_CanReadEntireMessage()
        {
            Dart380Helper.WriteFmt100Msg(_dart);

            _dart.SendKey(Dart380Key.ISK);
            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("291504*FR:CR    "));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("FRI*TEXT"));
            _dart.SendKey(Dart380Key.UARROW);
            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("                "));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("FRI*TEXT"));
            _dart.SendKey(Dart380Key.UARROW);
            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("TILL:JA         "));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("FRI*TEXT"));
            _dart.SendKey(Dart380Key.DARROW);
            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("                "));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("FRI*TEXT"));
            _dart.SendKey(Dart380Key.DARROW);
            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("291504*FR:CR    "));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("FRI*TEXT"));
            _dart.SendKey(Dart380Key.DARROW);
            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("                "));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("FRI*TEXT"));
            _dart.SendKey(Dart380Key.DARROW);
            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("FRÅN:     *U:   "));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("FRI*TEXT"));
            _dart.SendKey(Dart380Key.DARROW);
            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("TEXT:FÖRBERED RÖ"));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("FRI*TEXT"));
            _dart.SendKey(Dart380Key.DARROW);
            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("K 10 LAG        "));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("FRI*TEXT"));
            _dart.SendKey(Dart380Key.DARROW);
            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("                "));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("FRI*TEXT"));
            _dart.SendKey(Dart380Key.DARROW);
            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("                "));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("FRI*TEXT"));
            _dart.SendKey(Dart380Key.DARROW);
            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("                "));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("FRI*TEXT"));
            _dart.SendKey(Dart380Key.DARROW);
            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("                "));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("FRI*TEXT"));
            _dart.SendKey(Dart380Key.DARROW);
            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("                "));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("FRI*TEXT"));
            _dart.SendKey(Dart380Key.DARROW);
            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("                "));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("FRI*TEXT"));
            _dart.SendKey(Dart380Key.DARROW);
            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("                "));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("FRI*TEXT"));
            _dart.SendKey(Dart380Key.DARROW);
            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("                "));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("FRI*TEXT"));
            _dart.SendKey(Dart380Key.DARROW);
            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("                "));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("FRI*TEXT"));
            _dart.SendKey(Dart380Key.DARROW);
            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("                "));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("FRI*TEXT"));
            _dart.SendKey(Dart380Key.DARROW);
            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("------SLUT------"));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("FRI*TEXT"));
        }
    }
}