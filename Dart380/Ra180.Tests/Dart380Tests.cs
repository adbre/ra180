using NUnit.Framework;

namespace Ra180.Tests
{
    [TestFixture]
    public class Dart380Tests
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
        public void KanSändTextmeddelande()
        {
            _dart.SendKey(Ra180Key.TID);
            _dart.SendKey(Ra180Key.ÄND);
            _dart.SendKeys("150400");
            _dart.SendKey(Ra180Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:150400"));
            _dart.SendKey(Ra180Key.ENT);
            _dart.SendKey(Ra180Key.ÄND);
            _dart.SendKeys("0329");
            _dart.SendKey(Ra180Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("DAT:0329"));
            _dart.SendKey(Ra180Key.SLT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("        "));

            _dart.SendKey(Ra180Key.RDA);
            Assert.That(_dart.SmallDisplay.ToString(), Is.StringStarting("SDX"));
            _dart.SendKey(Ra180Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.StringMatching("BAT=[0-9]{2}.[0-9]"));
            _dart.SendKey(Ra180Key.SLT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("        "));

            _dart.SendKey(Ra180Key.KDA);
            _dart.SendKey(Ra180Key.ÄND);
            _dart.SendKeys("80450");
            _dart.SendKey(Ra180Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("FR:80450"));
            _dart.SendKey(Ra180Key.ENT);
            _dart.SendKey(Ra180Key.ÄND);
            _dart.SendKeys("4060");
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("BD1:4060"));
            _dart.SendKey(Ra180Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("BD2:    "));
            _dart.SendKey(Ra180Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("BD1:4060"));
            _dart.SendKey(Ra180Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("BD2:0000"));
            _dart.SendKey(Ra180Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("SYNK=NEJ"));
            _dart.SendKey(Ra180Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("PNY:### "));
            _dart.SendKey(Ra180Key.ÄND);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("PN1:    "));
            _dart.SendKeys("1111");
            _dart.SendKey(Ra180Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("PN2:    "));
            _dart.SendKeys("1111");
            _dart.SendKey(Ra180Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("PN3:    "));
            _dart.SendKeys("1111");
            _dart.SendKey(Ra180Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("PN4:    "));
            _dart.SendKeys("1111");
            _dart.SendKey(Ra180Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("PN5:    "));
            _dart.SendKeys("1111");
            _dart.SendKey(Ra180Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("PN6:    "));
            _dart.SendKeys("1111");
            _dart.SendKey(Ra180Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("PN7:    "));
            _dart.SendKeys("1111");
            _dart.SendKey(Ra180Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("PN8:    "));
            _dart.SendKeys("1111");
            _dart.SendKey(Ra180Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("PNY:000 "));
            _dart.SendKey(Ra180Key.SLT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("        "));

            _dart.SendKey(Ra180Key.NYK);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("NYK:### "));
            _dart.SendKey(Ra180Key.ÄND);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("NYK:000 "));
            _dart.SendKey(Ra180Key.SLT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("        "));

            _dart.SendKey(Ra180Key.EFF);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("EFF:LÅG "));
            _dart.SendKey(Ra180Key.ÄND);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("EFF:NORM"));
            _dart.SendKey(Ra180Key.SLT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("        "));
        }
        
    }
}