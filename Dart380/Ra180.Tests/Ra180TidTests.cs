using System;
using System.Linq;
using NUnit.Framework;

namespace Ra180.Tests
{
    [TestFixture]
    public class Ra180TidTests
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
        public void Should_be_1000_milliseconds_per_second()
        {
            _ra180.SendKey(Ra180Key.TID);
            _synchronizationContext.Tick(999);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:000000"));
            _synchronizationContext.Tick(1);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:000001"));
        }

        [Test]
        public void Should_be_59_seconds_per_minute()
        {
            _ra180.SendKey(Ra180Key.TID);
            _synchronizationContext.Tick(TimeSpan.FromSeconds(59));
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:000059"));
            _synchronizationContext.Tick(TimeSpan.FromSeconds(1));
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:000100"));
        }

        [Test]
        public void Should_be_59_minutes_per_hour()
        {
            _ra180.SendKey(Ra180Key.TID);
            _synchronizationContext.Tick(TimeSpan.FromMinutes(59));
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:005900"));
            _synchronizationContext.Tick(TimeSpan.FromMinutes(1));
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:010000"));
        }

        [Test]
        public void Should_be_24_hours_per_day()
        {
            _ra180.SendKey(Ra180Key.TID);
            _synchronizationContext.Tick(TimeSpan.FromHours(23));
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:230000"));
            _synchronizationContext.Tick(TimeSpan.FromHours(1));
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:000000"));
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("DAT:0102"));
        }

        [Test]
        public void Should_Cycle_Menu()
        {
            _ra180.SendKey(Ra180Key.TID);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:000000"));
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("DAT:0101"));
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.StringContaining("(TID)"));
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("        "));
        }

        [Test]
        public void EditTime_TimeElapses_DoesNotOverwriteUserInput()
        {
            _ra180.SendKey(Ra180Key.TID);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:000000"), "#1");
            _ra180.SendKey(Ra180Key.ÄND);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:      "), "#2");
            _ra180.SendKey(Ra180Key.Num1);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:1     "), "#3");

            _synchronizationContext.Tick(TimeSpan.FromSeconds(1));

            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:1     "), "#4");
        }

        [Test]
        public void EditTime_IgnoreInputAfterDisplayIsFilled()
        {
            _ra180.SendKey(Ra180Key.TID);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:000000"), "#1");
            _ra180.SendKey(Ra180Key.ÄND);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:      "), "#2");
            _ra180.SendKeys("123456");
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:123456"), "#8");

            _ra180.SendKey(Ra180Key.Num7);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:123456"), "#9");
        }

        [Test]
        public void EditTime_CanAbortEditBySLT()
        {
            _ra180.SendKey(Ra180Key.TID);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:000000"), "#1");
            _ra180.SendKey(Ra180Key.ÄND);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:      "), "#2");
            _ra180.SendKey(Ra180Key.Num1);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:1     "), "#3");

            _ra180.SendKey(Ra180Key.SLT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:000000"), "#1");
        }

        [Test]
        public void Time_CanCloseWithSLT()
        {
            _ra180.SendKey(Ra180Key.TID);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:000000"), "#1");
            _ra180.SendKey(Ra180Key.SLT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("        "), "#2");
        }

        [Test]
        public void Date_CanCloseWithSLT()
        {
            _ra180.SendKey(Ra180Key.TID);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:000000"), "#1");
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("DAT:0101"), "#1");
            _ra180.SendKey(Ra180Key.SLT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("        "), "#2");
        }

        [Test]
        public void ShouldOverwriteLastCharacter()
        {
            _ra180.SendKey(Ra180Key.TID);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:000000"), "#1");
            _ra180.SendKey(Ra180Key.ÄND);
            _ra180.SendKeys("121314");
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:121314"), "#2");
            _ra180.SendKeys("9");
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:121319"), "#3");
        }

        [Test]
        public void LastCharacterShouldFlashWithNoUnderscore()
        {
            _ra180.SendKey(Ra180Key.TID);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:000000"), "#1");
            _ra180.SendKey(Ra180Key.ÄND);
            _ra180.SendKeys("121314");
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:121314"), "#2");
            Assert.That(_ra180.Display.Characters.Last().HasUnderscore, Is.False, "#3 HAsUnderscore");
            Assert.That(_ra180.Display.Characters.Last().IsBlinking, Is.True, "#4 IsBlinking");
        }

        [Test]
        public void ShouldHaveUnderscoreOnlyAfterLastCharacter()
        {
            _ra180.SendKey(Ra180Key.TID);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:000000"), "#1");
            _ra180.SendKey(Ra180Key.ÄND);
            Assert.That(_ra180.Display.Characters.Skip(3).First().HasUnderscore, Is.True, "#3 HAsUnderscore");
            Assert.That(_ra180.Display.Characters.Count(c => c.HasUnderscore), Is.EqualTo(1), "#4 HasUnderscore - Count");
            _ra180.SendKey("1");
            Assert.That(_ra180.Display.Characters.Skip(4).First().HasUnderscore, Is.True, "#3 HAsUnderscore");
            Assert.That(_ra180.Display.Characters.Count(c => c.HasUnderscore), Is.EqualTo(1), "#4 HasUnderscore - Count");
        }

        [Test]
        public void CandEditTime()
        {
            _ra180.SendKey(Ra180Key.TID);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:000000"), "#1");
            _ra180.SendKey(Ra180Key.ÄND);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:      "), "#2");
            _ra180.SendKey(Ra180Key.Num1);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:1     "), "#3");
            _ra180.SendKey(Ra180Key.Num2);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:12    "), "#4");
            _ra180.SendKey(Ra180Key.Num3);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:123   "), "#5");
            _ra180.SendKey(Ra180Key.Num4);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:1234  "), "#6");
            _ra180.SendKey(Ra180Key.Num5);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:12345 "), "#7");
            _ra180.SendKey(Ra180Key.Num6);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:123456"), "#8");
            
            _ra180.SendKey(Ra180Key.Num7);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:123456"), "#9");

            _ra180.SendKey(Ra180Key.ÄND);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:12345 "), "#10");
            _ra180.SendKey(Ra180Key.ÄND);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:1234  "), "#11");
            _ra180.SendKey(Ra180Key.ÄND);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:123   "), "#12");
            _ra180.SendKey(Ra180Key.ÄND);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:12    "), "#13");
            _ra180.SendKey(Ra180Key.ÄND);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:1     "), "#14");
            _ra180.SendKey(Ra180Key.ÄND);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:      "), "#15");
            _ra180.SendKey(Ra180Key.ÄND);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:      "), "#16");

            _ra180.SendKey(Ra180Key.Asterix);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:      "), "#17");

            _ra180.SendKey(Ra180Key.NumberSign);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:      "), "#17");

            _ra180.SendKey(Ra180Key.Num1);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:1     "), "#18");
            _ra180.SendKey(Ra180Key.Num2);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:12    "), "#19");
            _ra180.SendKey(Ra180Key.Num3);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:123   "), "#20");
            _ra180.SendKey(Ra180Key.Num4);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:1234  "), "#21");
            _ra180.SendKey(Ra180Key.Num5);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:12345 "), "#22");
            _ra180.SendKey(Ra180Key.Num6);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:123456"), "#23");
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:123456"), "#24");
            _synchronizationContext.Tick(TimeSpan.FromSeconds(1));
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:123457"), "#25");
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("DAT:0101"), "#26");
            _ra180.SendKey(Ra180Key.SLT);
            _ra180.SendKey(Ra180Key.Num1);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:123457"), "#27");
        }

        [Test]
        public void Time_InvalidHour_ClearsInput()
        {
            _ra180.SendKeys(Ra180Key.TID, Ra180Key.ÄND);
            _ra180.SendKeys("250101");
            _ra180.SendKey(Ra180Key.ENT);

            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:      "), "#1");
        }

        [Test]
        public void Time_InvalidMinute_ClearsInput()
        {
            _ra180.SendKeys(Ra180Key.TID, Ra180Key.ÄND);
            _ra180.SendKeys("016001");
            _ra180.SendKey(Ra180Key.ENT);

            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:      "), "#1");
        }

        [Test]
        public void Time_InvalidSecond_ClearsInput()
        {
            _ra180.SendKeys(Ra180Key.TID, Ra180Key.ÄND);
            _ra180.SendKeys("010160");
            _ra180.SendKey(Ra180Key.ENT);

            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:      "), "#1");
        }

        [Test]
        public void Date_CanEdit()
        {
            _ra180.SendKeys(Ra180Key.TID, Ra180Key.ENT, Ra180Key.ÄND);
            _ra180.SendKeys("0320");
            _ra180.SendKey(Ra180Key.ENT);
            _ra180.SendKey(Ra180Key.SLT);
            _ra180.SendKeys(Ra180Key.TID, Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("DAT:0320"), "#1");
            _synchronizationContext.Tick(TimeSpan.FromDays(1));
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("DAT:0321"), "#1");
        }

        [Test]
        public void Date_InvalidMonth_ClearsInput()
        {
            _ra180.SendKeys(Ra180Key.TID, Ra180Key.ENT, Ra180Key.ÄND);
            _ra180.SendKeys("1301");
            _ra180.SendKey(Ra180Key.ENT);

            Assert.That(_ra180.Display.ToString(), Is.EqualTo("DAT:    "), "#1");
        }

        [Test]
        public void Date_InvalidDay_ClearsInput()
        {
            _ra180.SendKeys(Ra180Key.TID, Ra180Key.ENT, Ra180Key.ÄND);
            _ra180.SendKeys("0132");
            _ra180.SendKey(Ra180Key.ENT);

            Assert.That(_ra180.Display.ToString(), Is.EqualTo("DAT:    "), "#1");
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
            _ra180.SendKeys(Ra180Key.TID, Ra180Key.ENT, Ra180Key.ÄND);
            _ra180.SendKeys(input);
            _ra180.SendKey(Ra180Key.ENT);

            Assert.That(_ra180.Display.ToString(), Is.EqualTo("DAT:    "), "#1");
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
            _ra180.SendKeys(Ra180Key.TID, Ra180Key.ENT, Ra180Key.ÄND);
            _ra180.SendKeys(input);
            _ra180.SendKey(Ra180Key.ENT);

            Assert.That(_ra180.Display.ToString(), Is.EqualTo("DAT:" + input), "#1");
        }

        [Test]
        public void ShouldHave31DaysInJanuary()
        {
            _ra180.SendKeys(Ra180Key.TID, Ra180Key.ÄND);
            _ra180.SendKeys("235959");
            _ra180.SendKey(Ra180Key.ENT);
            _ra180.SendKey(Ra180Key.ENT);
            _ra180.SendKey(Ra180Key.ÄND);
            _ra180.SendKeys("0131");
            _ra180.SendKey(Ra180Key.ENT);

            _synchronizationContext.Tick(TimeSpan.FromSeconds(1));
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("DAT:0201"), "#1");
        }

        [Test]
        public void ShouldHave28DaysInFebruary()
        {
            _ra180.SendKeys(Ra180Key.TID, Ra180Key.ÄND);
            _ra180.SendKeys("235959");
            _ra180.SendKey(Ra180Key.ENT);
            _ra180.SendKey(Ra180Key.ENT);
            _ra180.SendKey(Ra180Key.ÄND);
            _ra180.SendKeys("0228");
            _ra180.SendKey(Ra180Key.ENT);

            _synchronizationContext.Tick(TimeSpan.FromSeconds(1));
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("DAT:0301"), "#1");
        }

        [Test]
        public void ShouldHave31DaysInMarch()
        {
            _ra180.SendKeys(Ra180Key.TID, Ra180Key.ÄND);
            _ra180.SendKeys("235959");
            _ra180.SendKey(Ra180Key.ENT);
            _ra180.SendKey(Ra180Key.ENT);
            _ra180.SendKey(Ra180Key.ÄND);
            _ra180.SendKeys("0331");
            _ra180.SendKey(Ra180Key.ENT);

            _synchronizationContext.Tick(TimeSpan.FromSeconds(1));
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("DAT:0401"), "#1");
        }

        [Test]
        public void ShouldHave30DaysInApril()
        {
            _ra180.SendKeys(Ra180Key.TID, Ra180Key.ÄND);
            _ra180.SendKeys("235959");
            _ra180.SendKey(Ra180Key.ENT);
            _ra180.SendKey(Ra180Key.ENT);
            _ra180.SendKey(Ra180Key.ÄND);
            _ra180.SendKeys("0430");
            _ra180.SendKey(Ra180Key.ENT);

            _synchronizationContext.Tick(TimeSpan.FromSeconds(1));
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("DAT:0501"), "#1");
        }

        [Test]
        public void ShouldHave31DaysInMay()
        {
            _ra180.SendKeys(Ra180Key.TID, Ra180Key.ÄND);
            _ra180.SendKeys("235959");
            _ra180.SendKey(Ra180Key.ENT);
            _ra180.SendKey(Ra180Key.ENT);
            _ra180.SendKey(Ra180Key.ÄND);
            _ra180.SendKeys("0531");
            _ra180.SendKey(Ra180Key.ENT);

            _synchronizationContext.Tick(TimeSpan.FromSeconds(1));
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("DAT:0601"), "#1");
        }

        [Test]
        public void ShouldHave30DaysInJune()
        {
            _ra180.SendKeys(Ra180Key.TID, Ra180Key.ÄND);
            _ra180.SendKeys("235959");
            _ra180.SendKey(Ra180Key.ENT);
            _ra180.SendKey(Ra180Key.ENT);
            _ra180.SendKey(Ra180Key.ÄND);
            _ra180.SendKeys("0630");
            _ra180.SendKey(Ra180Key.ENT);

            _synchronizationContext.Tick(TimeSpan.FromSeconds(1));
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("DAT:0701"), "#1");
        }

        [Test]
        public void ShouldHave31DaysInJuly()
        {
            _ra180.SendKeys(Ra180Key.TID, Ra180Key.ÄND);
            _ra180.SendKeys("235959");
            _ra180.SendKey(Ra180Key.ENT);
            _ra180.SendKey(Ra180Key.ENT);
            _ra180.SendKey(Ra180Key.ÄND);
            _ra180.SendKeys("0731");
            _ra180.SendKey(Ra180Key.ENT);

            _synchronizationContext.Tick(TimeSpan.FromSeconds(1));
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("DAT:0801"), "#1");
        }

        [Test]
        public void ShouldHave31DaysInAugust()
        {
            _ra180.SendKeys(Ra180Key.TID, Ra180Key.ÄND);
            _ra180.SendKeys("235959");
            _ra180.SendKey(Ra180Key.ENT);
            _ra180.SendKey(Ra180Key.ENT);
            _ra180.SendKey(Ra180Key.ÄND);
            _ra180.SendKeys("0831");
            _ra180.SendKey(Ra180Key.ENT);

            _synchronizationContext.Tick(TimeSpan.FromSeconds(1));
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("DAT:0901"), "#1");
        }

        [Test]
        public void ShouldHave30DaysInSeptember()
        {
            _ra180.SendKeys(Ra180Key.TID, Ra180Key.ÄND);
            _ra180.SendKeys("235959");
            _ra180.SendKey(Ra180Key.ENT);
            _ra180.SendKey(Ra180Key.ENT);
            _ra180.SendKey(Ra180Key.ÄND);
            _ra180.SendKeys("0930");
            _ra180.SendKey(Ra180Key.ENT);

            _synchronizationContext.Tick(TimeSpan.FromSeconds(1));
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("DAT:1001"), "#1");
        }

        [Test]
        public void ShouldHave31DaysInOctober()
        {
            _ra180.SendKeys(Ra180Key.TID, Ra180Key.ÄND);
            _ra180.SendKeys("235959");
            _ra180.SendKey(Ra180Key.ENT);
            _ra180.SendKey(Ra180Key.ENT);
            _ra180.SendKey(Ra180Key.ÄND);
            _ra180.SendKeys("1031");
            _ra180.SendKey(Ra180Key.ENT);

            _synchronizationContext.Tick(TimeSpan.FromSeconds(1));
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("DAT:1101"), "#1");
        }

        [Test]
        public void ShouldHave30DaysInNovember()
        {
            _ra180.SendKeys(Ra180Key.TID, Ra180Key.ÄND);
            _ra180.SendKeys("235959");
            _ra180.SendKey(Ra180Key.ENT);
            _ra180.SendKey(Ra180Key.ENT);
            _ra180.SendKey(Ra180Key.ÄND);
            _ra180.SendKeys("1130");
            _ra180.SendKey(Ra180Key.ENT);

            _synchronizationContext.Tick(TimeSpan.FromSeconds(1));
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("DAT:1201"), "#1");
        }

        [Test]
        public void ShouldHave31DaysInDecember()
        {
            _ra180.SendKeys(Ra180Key.TID, Ra180Key.ÄND);
            _ra180.SendKeys("235959");
            _ra180.SendKey(Ra180Key.ENT);
            _ra180.SendKey(Ra180Key.ENT);
            _ra180.SendKey(Ra180Key.ÄND);
            _ra180.SendKeys("1231");
            _ra180.SendKey(Ra180Key.ENT);

            _synchronizationContext.Tick(TimeSpan.FromSeconds(1));
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("DAT:0101"), "#1");
        }
    }
}