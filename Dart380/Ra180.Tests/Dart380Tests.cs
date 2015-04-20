using NUnit.Framework;
using Ra180.Devices.Dart380;

namespace Ra180.Tests
{
    [TestFixture]
    public class Dart380Tests
    {
        private FakeRadio _radio;
        private Dart380 _dart;
        private Ra180 _ra180;
        private DelayedSynchronizationContext _synchronizationContext;

        [SetUp]
        public void SetUp()
        {
            _radio = new FakeRadio();
            _synchronizationContext = new DelayedSynchronizationContext();
            _ra180 = new Ra180(_radio, _synchronizationContext);
            _dart = new Dart380(_synchronizationContext) { Ra180 = _ra180 };

            _dart.SendKey(Ra180Key.ModSKYDD);
            _ra180.SendKey(Dart380Key.ModSKYDD);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
        }

        [Test]
        public void KanSändTextmeddelande()
        {
            _dart.SendKey(Dart380Key.TID);
            _dart.SendKey(Dart380Key.ÄND);
            _dart.SendKeys("150400");
            _dart.SendKey(Dart380Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:150400"));
            _dart.SendKey(Dart380Key.ENT);
            _dart.SendKey(Dart380Key.ÄND);
            _dart.SendKeys("0329");
            _dart.SendKey(Dart380Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("DAT:0329"));
            _dart.SendKey(Dart380Key.SLT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("        "));

            _dart.SendKey(Dart380Key.RDA);
            Assert.That(_dart.SmallDisplay.ToString(), Is.StringStarting("SDX"));
            _dart.SendKey(Dart380Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.StringMatching("BAT=[0-9]{2}.[0-9]"));
            _dart.SendKey(Dart380Key.SLT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("        "));

            _dart.SendKey(Dart380Key.KDA);
            _dart.SendKey(Dart380Key.ÄND);
            _dart.SendKeys("80450");
            _dart.SendKey(Dart380Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("FR:80450"));
            _dart.SendKey(Dart380Key.ENT);
            _dart.SendKey(Dart380Key.ÄND);
            _dart.SendKeys("4060");
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("BD1:4060"));
            _dart.SendKey(Dart380Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("BD2:    "));
            _dart.SendKey(Dart380Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("BD1:4060"));
            _dart.SendKey(Dart380Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("BD2:0000"));
            _dart.SendKey(Dart380Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("SYNK=NEJ"));
            _dart.SendKey(Dart380Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("PNY:### "));
            _dart.SendKey(Dart380Key.ÄND);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("PN1:    "));
            _dart.SendKeys("1111");
            _dart.SendKey(Dart380Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("PN2:    "));
            _dart.SendKeys("1111");
            _dart.SendKey(Dart380Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("PN3:    "));
            _dart.SendKeys("1111");
            _dart.SendKey(Dart380Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("PN4:    "));
            _dart.SendKeys("1111");
            _dart.SendKey(Dart380Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("PN5:    "));
            _dart.SendKeys("1111");
            _dart.SendKey(Dart380Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("PN6:    "));
            _dart.SendKeys("1111");
            _dart.SendKey(Dart380Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("PN7:    "));
            _dart.SendKeys("1111");
            _dart.SendKey(Dart380Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("PN8:    "));
            _dart.SendKeys("1111");
            _dart.SendKey(Dart380Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("PNY:000 "));
            _dart.SendKey(Dart380Key.SLT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("        "));

            _dart.SendKey(Dart380Key.NYK);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("NYK:### "));
            _dart.SendKey(Dart380Key.ÄND);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("NYK:000 "));
            _dart.SendKey(Dart380Key.SLT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("        "));

            _dart.SendKey(Dart380Key.EFF);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("EFF:LÅG "));
            _dart.SendKey(Dart380Key.ÄND);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("EFF:NORM"));
            _dart.SendKey(Dart380Key.SLT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("        "));

            _dart.SendKey(Dart380Key.DDA);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("AD:*    "));
            _dart.SendKey(Dart380Key.ÄND);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("AD:     "));
            _dart.SendKeys("CR");
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("AD:CR   "));
            _dart.SendKey(Dart380Key.ENT); // SAVE
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("AD:CR   "));
            _dart.SendKey(Dart380Key.ENT); // NEXT
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("SKR:MAN "));
            _dart.SendKey(Dart380Key.ÄND);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("SKR:MOT "));
            _dart.SendKey(Dart380Key.ÄND);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("SKR:ALLA"));
            _dart.SendKey(Dart380Key.ÄND);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("SKR:AVS "));
            _dart.SendKey(Dart380Key.ÄND);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("SKR:MAN "));
            _dart.SendKey(Dart380Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("OPMTN:PÅ"));
            _dart.SendKey(Dart380Key.ÄND);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("OPMTN:AV"));
            _dart.SendKey(Dart380Key.ÄND);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("OPMTN:PÅ"));
            _dart.SendKey(Dart380Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("SUM:TILL"));
            _dart.SendKey(Dart380Key.ÄND);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("SUM:FRÅN"));
            _dart.SendKey(Dart380Key.ÄND);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("SUM:TILL"));
            _dart.SendKey(Dart380Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("TKL:TILL"));
            _dart.SendKey(Dart380Key.ÄND);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("TKL:FRÅN"));
            _dart.SendKey(Dart380Key.ÄND);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("TKL:TILL"));
            _dart.SendKey(Dart380Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.StringMatching(@"^BAT=[0-9]{2}\.[0-9]$"));
            _dart.SendKey(Dart380Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.StringContaining("(DDA)"));
            _dart.SendKey(Dart380Key.SLT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("        "));


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
            
            _dart.SendKey(Dart380Key.ISK);
            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("291504*FR:CR    "));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("FRI*TEXT"));
            _dart.SendKey(Dart380Key.SND);
            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("     SÄNDER     "));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("FRI*TEXT"));
            Assert.That(_radio.PendingMessage.Data, Is.EqualTo(new[]
            {
                "TILL:JA         ",
                "                ",
                "291504*FR:CR    ",
                "                ",
                "FRÅN:     *U:   ",
                "TEXT:FÖRBERED RÖ", // 1
                "K 10 LAG        ", // 2
                "                ", // 3
                "                ", // 4
                "                ", // 5
                "                ", // 6
                "                ", // 7
                "                ", // 8
                "                ", // 9
                "                ", // 10
                "                ", // 11
                "                ", // 12
                "------SLUT------",
            }));
            _radio.PendingMessage.MarkAsSent();
            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("      SÄNT      "));
            _dart.SendKey(Dart380Key.SLT);
            Assert.That(_dart.LargeDisplay.ToString(), Is.EqualTo("                "));

        }
    }
}