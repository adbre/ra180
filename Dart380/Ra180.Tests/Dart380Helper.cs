using NUnit.Framework;
using Ra180.Devices.Dart380;

namespace Ra180.Tests
{
    public static class Dart380Helper
    {
        public static void SetUp(Dart380 dart)
        {
            dart.SendKey(Dart380Key.TID);
            dart.SendKey(Dart380Key.ÄND);
            dart.SendKeys("150400");
            dart.SendKey(Dart380Key.ENT);
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("T:150400"));
            dart.SendKey(Dart380Key.ENT);
            dart.SendKey(Dart380Key.ÄND);
            dart.SendKeys("0329");
            dart.SendKey(Dart380Key.ENT);
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("DAT:0329"));
            dart.SendKey(Dart380Key.SLT);
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("        "));

            dart.SendKey(Dart380Key.RDA);
            Assert.That(dart.SmallDisplay.ToString(), Is.StringStarting("SDX"));
            dart.SendKey(Dart380Key.ENT);
            Assert.That(dart.SmallDisplay.ToString(), Is.StringMatching("BAT=[0-9]{2}.[0-9]"));
            dart.SendKey(Dart380Key.SLT);
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("        "));

            dart.SendKey(Dart380Key.KDA);
            dart.SendKey(Dart380Key.ÄND);
            dart.SendKeys("80450");
            dart.SendKey(Dart380Key.ENT);
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("FR:80450"));
            dart.SendKey(Dart380Key.ENT);
            dart.SendKey(Dart380Key.ÄND);
            dart.SendKeys("4060");
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("BD1:4060"));
            dart.SendKey(Dart380Key.ENT);
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("BD2:    "));
            dart.SendKey(Dart380Key.ENT);
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("BD1:4060"));
            dart.SendKey(Dart380Key.ENT);
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("BD2:0000"));
            dart.SendKey(Dart380Key.ENT);
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("SYNK=NEJ"));
            dart.SendKey(Dart380Key.ENT);
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("PNY:### "));
            dart.SendKey(Dart380Key.ÄND);
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("PN1:    "));
            dart.SendKeys("1111");
            dart.SendKey(Dart380Key.ENT);
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("PN2:    "));
            dart.SendKeys("1111");
            dart.SendKey(Dart380Key.ENT);
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("PN3:    "));
            dart.SendKeys("1111");
            dart.SendKey(Dart380Key.ENT);
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("PN4:    "));
            dart.SendKeys("1111");
            dart.SendKey(Dart380Key.ENT);
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("PN5:    "));
            dart.SendKeys("1111");
            dart.SendKey(Dart380Key.ENT);
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("PN6:    "));
            dart.SendKeys("1111");
            dart.SendKey(Dart380Key.ENT);
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("PN7:    "));
            dart.SendKeys("1111");
            dart.SendKey(Dart380Key.ENT);
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("PN8:    "));
            dart.SendKeys("1111");
            dart.SendKey(Dart380Key.ENT);
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("PNY:000 "));
            dart.SendKey(Dart380Key.SLT);
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("        "));

            dart.SendKey(Dart380Key.NYK);
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("NYK:### "));
            dart.SendKey(Dart380Key.ÄND);
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("NYK:000 "));
            dart.SendKey(Dart380Key.SLT);
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("        "));

            dart.SendKey(Dart380Key.EFF);
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("EFF:LÅG "));
            dart.SendKey(Dart380Key.ÄND);
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("EFF:NORM"));
            dart.SendKey(Dart380Key.SLT);
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("        "));

            dart.SendKey(Dart380Key.DDA);
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("AD:*    "));
            dart.SendKey(Dart380Key.ÄND);
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("AD:     "));
            dart.SendKeys("CR");
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("AD:CR   "));
            dart.SendKey(Dart380Key.ENT); // SAVE
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("AD:CR   "));
            dart.SendKey(Dart380Key.ENT); // NEXT
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("SKR:MAN "));
            dart.SendKey(Dart380Key.ÄND);
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("SKR:MOT "));
            dart.SendKey(Dart380Key.ÄND);
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("SKR:ALLA"));
            dart.SendKey(Dart380Key.ÄND);
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("SKR:AVS "));
            dart.SendKey(Dart380Key.ÄND);
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("SKR:MAN "));
            dart.SendKey(Dart380Key.ENT);
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("OPMTN:PÅ"));
            dart.SendKey(Dart380Key.ÄND);
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("OPMTN:AV"));
            dart.SendKey(Dart380Key.ÄND);
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("OPMTN:PÅ"));
            dart.SendKey(Dart380Key.ENT);
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("SUM:TILL"));
            dart.SendKey(Dart380Key.ÄND);
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("SUM:FRÅN"));
            dart.SendKey(Dart380Key.ÄND);
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("SUM:TILL"));
            dart.SendKey(Dart380Key.ENT);
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("TKL:TILL"));
            dart.SendKey(Dart380Key.ÄND);
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("TKL:FRÅN"));
            dart.SendKey(Dart380Key.ÄND);
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("TKL:TILL"));
            dart.SendKey(Dart380Key.ENT);
            Assert.That(dart.SmallDisplay.ToString(), Is.StringMatching(@"^BAT=[0-9]{2}\.[0-9]$"));
            dart.SendKey(Dart380Key.ENT);
            Assert.That(dart.SmallDisplay.ToString(), Is.StringContaining("(DDA)"));
            dart.SendKey(Dart380Key.SLT);
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("        "));
        }

        public static void WriteFmt100Msg(Dart380 dart)
        {
            dart.SendKey(Dart380Key.FMT);
            Assert.That(dart.LargeDisplay.ToString(), Is.EqualTo("FORMAT:         "));
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("ROT*NIVÅ"));
            dart.SendKey("1");
            Assert.That(dart.LargeDisplay.ToString(), Is.EqualTo("FORMAT:1        "));
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("LEDNING "));
            dart.SendKey("0");
            Assert.That(dart.LargeDisplay.ToString(), Is.EqualTo("FORMAT:10       "));
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("LEDNSBTJ"));
            dart.SendKey("0");
            Assert.That(dart.LargeDisplay.ToString(), Is.EqualTo("FORMAT:100      "));
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("FRI*TEXT"));
            dart.SendKey(Dart380Key.ENT);
            Assert.That(dart.LargeDisplay.ToString(), Is.EqualTo("FRI*TEXT*       "));
            Assert.That(dart.SmallDisplay.ToString(), Is.EqualTo("FRI*TEXT"));
            dart.SendKey(Dart380Key.ENT);
            Assert.That(dart.LargeDisplay.ToString(), Is.EqualTo("TILL:           "));
            dart.SendKey(Dart380Key.ÄND);
            dart.SendKeys("JA");
            Assert.That(dart.LargeDisplay.ToString(), Is.EqualTo("TILL:JA         "));
            dart.SendKey(Dart380Key.ENT);
            Assert.That(dart.LargeDisplay.ToString(), Is.EqualTo("                "));
            dart.SendKey(Dart380Key.ENT);
            Assert.That(dart.LargeDisplay.ToString(), Is.EqualTo("291504*FR:CR    "));
            dart.SendKey(Dart380Key.ENT);
            Assert.That(dart.LargeDisplay.ToString(), Is.EqualTo("                "));
            dart.SendKey(Dart380Key.ENT);
            Assert.That(dart.LargeDisplay.ToString(), Is.EqualTo("FRÅN:     *U:   "));
            dart.SendKey(Dart380Key.ENT);
            Assert.That(dart.LargeDisplay.ToString(), Is.EqualTo("TEXT:           "));
            dart.SendKeys("FÖRBERED RÖ");
            Assert.That(dart.LargeDisplay.ToString(), Is.EqualTo("TEXT:FÖRBERED RÖ"));
            dart.SendKeys("K 10 LAG");
            Assert.That(dart.LargeDisplay.ToString(), Is.EqualTo("K 10 LAG        "));
            dart.SendKey(Dart380Key.SLT);
            Assert.That(dart.LargeDisplay.ToString(), Is.EqualTo("K 10 LAG        "));
            dart.SendKey(Dart380Key.SLT);
            Assert.That(dart.LargeDisplay.ToString(), Is.EqualTo("                "));
        }
    }
}