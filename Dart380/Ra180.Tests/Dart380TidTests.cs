using System;
using NUnit.Framework;

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

    [TestFixture]
    public class Dart380TidTests
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

            _dart.SendKey(Ra180Key.ModKLAR);
            _ra180.SendKey(Ra180Key.ModKLAR);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
        }

        [Test]
        public void Should_be_1000_milliseconds_per_second()
        {
            _dart.SendKey(Ra180Key.TID);
            _synchronizationContext.Tick(999);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:000000"));
            _synchronizationContext.Tick(1);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:000001"));
        }

        [Test]
        public void Should_be_59_seconds_per_minute()
        {
            _dart.SendKey(Ra180Key.TID);
            _synchronizationContext.Tick(TimeSpan.FromSeconds(59));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:000059"));
            _synchronizationContext.Tick(TimeSpan.FromSeconds(1));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:000100"));
        }

        [Test]
        public void Should_be_59_minutes_per_hour()
        {
            _dart.SendKey(Ra180Key.TID);
            _synchronizationContext.Tick(TimeSpan.FromMinutes(59));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:005900"));
            _synchronizationContext.Tick(TimeSpan.FromMinutes(1));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:010000"));
        }

        [Test]
        public void Should_be_24_hours_per_day()
        {
            _dart.SendKey(Ra180Key.TID);
            _synchronizationContext.Tick(TimeSpan.FromHours(23));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:230000"));
            _synchronizationContext.Tick(TimeSpan.FromHours(1));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:000000"));
            _dart.SendKey(Ra180Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("DAT:0102"));
        }

        [Test]
        public void Should_Cycle_Menu()
        {
            _dart.SendKey(Ra180Key.TID);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:000000"));
            _dart.SendKey(Ra180Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("DAT:0101"));
            _dart.SendKey(Ra180Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.StringContaining("(TID)"));
            _dart.SendKey(Ra180Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("        "));
        }

        [Test]
        public void EditTime_TimeElapses_DoesNotOverwriteUserInput()
        {
            _dart.SendKey(Ra180Key.TID);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:000000"), "#1");
            _dart.SendKey(Ra180Key.ÄND);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:      "), "#2");
            _dart.SendKey(Ra180Key.Num1);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:1     "), "#3");

            _synchronizationContext.Tick(TimeSpan.FromSeconds(1));

            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:1     "), "#4");
        }

        [Test]
        public void EditTime_OverwriteLastCharacterAfterDisplayIsFilled()
        {
            _dart.SendKey(Ra180Key.TID);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:000000"), "#1");
            _dart.SendKey(Ra180Key.ÄND);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:      "), "#2");
            _dart.SendKeys("123456");
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:123456"), "#8");

            _dart.SendKey(Ra180Key.Num7);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:123457"), "#9");
        }

        [Test]
        public void EditTime_CanAbortEditBySLT()
        {
            _dart.SendKey(Ra180Key.TID);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:000000"), "#1");
            _dart.SendKey(Ra180Key.ÄND);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:      "), "#2");
            _dart.SendKey(Ra180Key.Num1);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:1     "), "#3");

            _dart.SendKey(Ra180Key.SLT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:000000"), "#1");
        }

        [Test]
        public void Time_CanCloseWithSLT()
        {
            _dart.SendKey(Ra180Key.TID);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:000000"), "#1");
            _dart.SendKey(Ra180Key.SLT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("        "), "#2");
        }

        [Test]
        public void Date_CanCloseWithSLT()
        {
            _dart.SendKey(Ra180Key.TID);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:000000"), "#1");
            _dart.SendKey(Ra180Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("DAT:0101"), "#1");
            _dart.SendKey(Ra180Key.SLT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("        "), "#2");
        }

        [Test]
        public void CandEditTime()
        {
            _dart.SendKey(Ra180Key.TID);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:000000"), "#1");
            _dart.SendKey(Ra180Key.ÄND);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:      "), "#2");
            _dart.SendKey(Ra180Key.Num1);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:1     "), "#3");
            _dart.SendKey(Ra180Key.Num2);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:12    "), "#4");
            _dart.SendKey(Ra180Key.Num3);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:123   "), "#5");
            _dart.SendKey(Ra180Key.Num4);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:1234  "), "#6");
            _dart.SendKey(Ra180Key.Num5);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:12345 "), "#7");
            _dart.SendKey(Ra180Key.Num6);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:123456"), "#8");

            _dart.SendKey(Ra180Key.ÄND);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:12345 "), "#10");
            _dart.SendKey(Ra180Key.ÄND);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:1234  "), "#11");
            _dart.SendKey(Ra180Key.ÄND);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:123   "), "#12");
            _dart.SendKey(Ra180Key.ÄND);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:12    "), "#13");
            _dart.SendKey(Ra180Key.ÄND);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:1     "), "#14");
            _dart.SendKey(Ra180Key.ÄND);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:      "), "#15");
            _dart.SendKey(Ra180Key.ÄND);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:      "), "#16");

            _dart.SendKey(Ra180Key.Asterix);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:      "), "#17");

            _dart.SendKey(Ra180Key.NumberSign);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:      "), "#17");

            _dart.SendKey(Ra180Key.Num1);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:1     "), "#18");
            _dart.SendKey(Ra180Key.Num2);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:12    "), "#19");
            _dart.SendKey(Ra180Key.Num3);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:123   "), "#20");
            _dart.SendKey(Ra180Key.Num4);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:1234  "), "#21");
            _dart.SendKey(Ra180Key.Num5);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:12345 "), "#22");
            _dart.SendKey(Ra180Key.Num6);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:123456"), "#23");
            _dart.SendKey(Ra180Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:123456"), "#24");
            _synchronizationContext.Tick(TimeSpan.FromSeconds(1));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:123457"), "#25");
            _dart.SendKey(Ra180Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("DAT:0101"), "#26");
            _dart.SendKey(Ra180Key.SLT);
            _dart.SendKey(Ra180Key.Num1);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:123457"), "#27");
        }

        [Test]
        public void Time_InvalidHour_ClearsInput()
        {
            _dart.SendKeys(Ra180Key.TID, Ra180Key.ÄND);
            _dart.SendKeys("250101");
            _dart.SendKey(Ra180Key.ENT);

            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:      "), "#1");
        }

        [Test]
        public void Time_InvalidMinute_ClearsInput()
        {
            _dart.SendKeys(Ra180Key.TID, Ra180Key.ÄND);
            _dart.SendKeys("016001");
            _dart.SendKey(Ra180Key.ENT);

            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:      "), "#1");
        }

        [Test]
        public void Time_InvalidSecond_ClearsInput()
        {
            _dart.SendKeys(Ra180Key.TID, Ra180Key.ÄND);
            _dart.SendKeys("010160");
            _dart.SendKey(Ra180Key.ENT);

            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("T:      "), "#1");
        }

        [Test]
        public void Date_CanEdit()
        {
            _dart.SendKeys(Ra180Key.TID, Ra180Key.ENT, Ra180Key.ÄND);
            _dart.SendKeys("0320");
            _dart.SendKey(Ra180Key.ENT);
            _dart.SendKey(Ra180Key.SLT);
            _dart.SendKeys(Ra180Key.TID, Ra180Key.ENT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("DAT:0320"), "#1");
            _synchronizationContext.Tick(TimeSpan.FromDays(1));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("DAT:0321"), "#1");
        }

        [Test]
        public void Date_InvalidMonth_ClearsInput()
        {
            _dart.SendKeys(Ra180Key.TID, Ra180Key.ENT, Ra180Key.ÄND);
            _dart.SendKeys("1301");
            _dart.SendKey(Ra180Key.ENT);

            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("DAT:    "), "#1");
        }

        [Test]
        public void Date_InvalidDay_ClearsInput()
        {
            _dart.SendKeys(Ra180Key.TID, Ra180Key.ENT, Ra180Key.ÄND);
            _dart.SendKeys("0132");
            _dart.SendKey(Ra180Key.ENT);

            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("DAT:    "), "#1");
        }

        [Test]
        [TestCase("0132")]
        [TestCase("0229")]
        [TestCase("0332")]
        [TestCase("0431")]
        [TestCase("0532")]
        [TestCase("0631")]
        [TestCase("0732")]
        [TestCase("0832")]
        [TestCase("0931")]
        [TestCase("1032")]
        [TestCase("1131")]
        [TestCase("1232")]
        public void Date_InvalidDayInMonth_ClearsInput(string input)
        {
            _dart.SendKeys(Ra180Key.TID, Ra180Key.ENT, Ra180Key.ÄND);
            _dart.SendKeys(input);
            _dart.SendKey(Ra180Key.ENT);

            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("DAT:    "), "#1");
        }

        [Test]
        [TestCase("0131")]
        [TestCase("0228")]
        [TestCase("0331")]
        [TestCase("0430")]
        [TestCase("0531")]
        [TestCase("0630")]
        [TestCase("0731")]
        [TestCase("0831")]
        [TestCase("0930")]
        [TestCase("1031")]
        [TestCase("1130")]
        [TestCase("1231")]
        public void Date_ValidDayInMonth_SaveInput(string input)
        {
            _dart.SendKeys(Ra180Key.TID, Ra180Key.ENT, Ra180Key.ÄND);
            _dart.SendKeys(input);
            _dart.SendKey(Ra180Key.ENT);

            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("DAT:" + input), "#1");
        }

        [Test]
        public void ShouldHave31DaysInJanuary()
        {
            _dart.SendKeys(Ra180Key.TID, Ra180Key.ÄND);
            _dart.SendKeys("235959");
            _dart.SendKey(Ra180Key.ENT);
            _dart.SendKey(Ra180Key.ENT);
            _dart.SendKey(Ra180Key.ÄND);
            _dart.SendKeys("0131");
            _dart.SendKey(Ra180Key.ENT);

            _synchronizationContext.Tick(TimeSpan.FromSeconds(1));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("DAT:0201"), "#1");
        }

        [Test]
        public void ShouldHave28DaysInFebruary()
        {
            _dart.SendKeys(Ra180Key.TID, Ra180Key.ÄND);
            _dart.SendKeys("235959");
            _dart.SendKey(Ra180Key.ENT);
            _dart.SendKey(Ra180Key.ENT);
            _dart.SendKey(Ra180Key.ÄND);
            _dart.SendKeys("0228");
            _dart.SendKey(Ra180Key.ENT);

            _synchronizationContext.Tick(TimeSpan.FromSeconds(1));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("DAT:0301"), "#1");
        }

        [Test]
        public void ShouldHave31DaysInMarch()
        {
            _dart.SendKeys(Ra180Key.TID, Ra180Key.ÄND);
            _dart.SendKeys("235959");
            _dart.SendKey(Ra180Key.ENT);
            _dart.SendKey(Ra180Key.ENT);
            _dart.SendKey(Ra180Key.ÄND);
            _dart.SendKeys("0331");
            _dart.SendKey(Ra180Key.ENT);

            _synchronizationContext.Tick(TimeSpan.FromSeconds(1));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("DAT:0401"), "#1");
        }

        [Test]
        public void ShouldHave30DaysInApril()
        {
            _dart.SendKeys(Ra180Key.TID, Ra180Key.ÄND);
            _dart.SendKeys("235959");
            _dart.SendKey(Ra180Key.ENT);
            _dart.SendKey(Ra180Key.ENT);
            _dart.SendKey(Ra180Key.ÄND);
            _dart.SendKeys("0430");
            _dart.SendKey(Ra180Key.ENT);

            _synchronizationContext.Tick(TimeSpan.FromSeconds(1));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("DAT:0501"), "#1");
        }

        [Test]
        public void ShouldHave31DaysInMay()
        {
            _dart.SendKeys(Ra180Key.TID, Ra180Key.ÄND);
            _dart.SendKeys("235959");
            _dart.SendKey(Ra180Key.ENT);
            _dart.SendKey(Ra180Key.ENT);
            _dart.SendKey(Ra180Key.ÄND);
            _dart.SendKeys("0531");
            _dart.SendKey(Ra180Key.ENT);

            _synchronizationContext.Tick(TimeSpan.FromSeconds(1));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("DAT:0601"), "#1");
        }

        [Test]
        public void ShouldHave30DaysInJune()
        {
            _dart.SendKeys(Ra180Key.TID, Ra180Key.ÄND);
            _dart.SendKeys("235959");
            _dart.SendKey(Ra180Key.ENT);
            _dart.SendKey(Ra180Key.ENT);
            _dart.SendKey(Ra180Key.ÄND);
            _dart.SendKeys("0630");
            _dart.SendKey(Ra180Key.ENT);

            _synchronizationContext.Tick(TimeSpan.FromSeconds(1));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("DAT:0701"), "#1");
        }

        [Test]
        public void ShouldHave31DaysInJuly()
        {
            _dart.SendKeys(Ra180Key.TID, Ra180Key.ÄND);
            _dart.SendKeys("235959");
            _dart.SendKey(Ra180Key.ENT);
            _dart.SendKey(Ra180Key.ENT);
            _dart.SendKey(Ra180Key.ÄND);
            _dart.SendKeys("0731");
            _dart.SendKey(Ra180Key.ENT);

            _synchronizationContext.Tick(TimeSpan.FromSeconds(1));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("DAT:0801"), "#1");
        }

        [Test]
        public void ShouldHave31DaysInAugust()
        {
            _dart.SendKeys(Ra180Key.TID, Ra180Key.ÄND);
            _dart.SendKeys("235959");
            _dart.SendKey(Ra180Key.ENT);
            _dart.SendKey(Ra180Key.ENT);
            _dart.SendKey(Ra180Key.ÄND);
            _dart.SendKeys("0831");
            _dart.SendKey(Ra180Key.ENT);

            _synchronizationContext.Tick(TimeSpan.FromSeconds(1));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("DAT:0901"), "#1");
        }

        [Test]
        public void ShouldHave30DaysInSeptember()
        {
            _dart.SendKeys(Ra180Key.TID, Ra180Key.ÄND);
            _dart.SendKeys("235959");
            _dart.SendKey(Ra180Key.ENT);
            _dart.SendKey(Ra180Key.ENT);
            _dart.SendKey(Ra180Key.ÄND);
            _dart.SendKeys("0930");
            _dart.SendKey(Ra180Key.ENT);

            _synchronizationContext.Tick(TimeSpan.FromSeconds(1));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("DAT:1001"), "#1");
        }

        [Test]
        public void ShouldHave31DaysInOctober()
        {
            _dart.SendKeys(Ra180Key.TID, Ra180Key.ÄND);
            _dart.SendKeys("235959");
            _dart.SendKey(Ra180Key.ENT);
            _dart.SendKey(Ra180Key.ENT);
            _dart.SendKey(Ra180Key.ÄND);
            _dart.SendKeys("1031");
            _dart.SendKey(Ra180Key.ENT);

            _synchronizationContext.Tick(TimeSpan.FromSeconds(1));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("DAT:1101"), "#1");
        }

        [Test]
        public void ShouldHave30DaysInNovember()
        {
            _dart.SendKeys(Ra180Key.TID, Ra180Key.ÄND);
            _dart.SendKeys("235959");
            _dart.SendKey(Ra180Key.ENT);
            _dart.SendKey(Ra180Key.ENT);
            _dart.SendKey(Ra180Key.ÄND);
            _dart.SendKeys("1130");
            _dart.SendKey(Ra180Key.ENT);

            _synchronizationContext.Tick(TimeSpan.FromSeconds(1));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("DAT:1201"), "#1");
        }

        [Test]
        public void ShouldHave31DaysInDecember()
        {
            _dart.SendKeys(Ra180Key.TID, Ra180Key.ÄND);
            _dart.SendKeys("235959");
            _dart.SendKey(Ra180Key.ENT);
            _dart.SendKey(Ra180Key.ENT);
            _dart.SendKey(Ra180Key.ÄND);
            _dart.SendKeys("1231");
            _dart.SendKey(Ra180Key.ENT);

            _synchronizationContext.Tick(TimeSpan.FromSeconds(1));
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("DAT:0101"), "#1");
        }
    }
}
