using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Ra180.Tests
{
    [TestFixture]
    public class Ra180_TidTests
    {
        private Ra180 _ra180;
        private DelayedSynchronizationContext _synchronizationContext;

        [SetUp]
        public void SetUp()
        {
            _synchronizationContext = new DelayedSynchronizationContext();
            _ra180 = new Ra180(_synchronizationContext);

            _ra180.SendKey(Ra180KeyCode.ModKLAR);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
        }

        [Test]
        public void Should_be_1000_milliseconds_per_second()
        {
            _ra180.SendKey(Ra180KeyCode.TID);
            _synchronizationContext.Tick(999);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:000000"));
            _synchronizationContext.Tick(1);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:000001"));
        }
    }

    [TestFixture]
    public class Ra180_SelfTestTests
    {
        private Ra180 _ra180;
        private DelayedSynchronizationContext _synchronizationContext;

        [SetUp]
        public void SetUp()
        {
            _synchronizationContext = new DelayedSynchronizationContext();
            _ra180 = new Ra180(_synchronizationContext);
        }

        [Test]
        public void ShouldPerformSelfTestOnKLAR()
        {
            _ra180.SendKey(Ra180KeyCode.ModKLAR);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("TEST    "));
            
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("TEST OK "));

            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("NOLLST  "));

            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("        "));
        }

        [Test]
        public void ShouldNotDelayChangeOfModToKLAR()
        {
            _ra180.SendKey(Ra180KeyCode.ModKLAR);
            Assert.That(_ra180.Mod, Is.EqualTo(Ra180Mod.KLAR));
        }

        [Test]
        public void ShouldNotDelayChangeOfModToSKYDD()
        {
            _ra180.SendKey(Ra180KeyCode.ModSKYDD);
            Assert.That(_ra180.Mod, Is.EqualTo(Ra180Mod.SKYDD));
        }

        [Test]
        public void ShouldNotDelayChangeOfModToDRELÄ()
        {
            _ra180.SendKey(Ra180KeyCode.ModDRELÄ);
            Assert.That(_ra180.Mod, Is.EqualTo(Ra180Mod.DRELÄ));
        }

        [Test]
        public void ShouldNotAcceptInputUntilSelfTestIsComplete()
        {
            _ra180.SendKey(Ra180KeyCode.ModKLAR);
            _ra180.SendKey(Ra180KeyCode.Num1);
            Assert.That(_ra180.Display, Is.Not.StringMatching("^T:"));
        }

        [Test]
        public void ShouldNotDisplayNOLLSTIfEnteredKDA()
        {
            _ra180.SendKey(Ra180KeyCode.ModSKYDD);
            _synchronizationContext.Tick(Ra180.SELFTEST);
            Helper.EnterNewPny(_ra180);

            _ra180.SendKey(Ra180KeyCode.ModFR);
            _ra180.SendKey(Ra180KeyCode.ModSKYDD);

            Assert.That(_ra180.Display.ToString(), Is.EqualTo("TEST    "));

            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("TEST OK "));

            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("        "));
        }

        [Test]
        public void ShouldPerformSelfTestAfterReset()
        {
            _ra180.SendKey(Ra180KeyCode.ModSKYDD);
            _synchronizationContext.Tick(Ra180.SELFTEST);

            _ra180.SendKey(Ra180KeyCode.Asterix | Ra180KeyCode.NumberSign);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("TEST    "));

            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("TEST OK "));

            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("NOLLST  "));

            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("        "));
        }

        [Test]
        public void ShouldCancelSelfTestFromTESTWhenOFF()
        {
            _ra180.SendKey(Ra180KeyCode.ModKLAR);
            _ra180.SendKey(Ra180KeyCode.ModFR);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("        "));
        }

        [Test]
        public void ShouldCancelSelfTestFromTESTOKWhenOFF()
        {
            _ra180.SendKey(Ra180KeyCode.ModKLAR);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            _ra180.SendKey(Ra180KeyCode.ModFR);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("        "));
        }

        [Test]
        public void ShouldCancelSelfTestFromNOLLSTWhenOFF()
        {
            _ra180.SendKey(Ra180KeyCode.ModKLAR);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            _ra180.SendKey(Ra180KeyCode.ModFR);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("        "));
        }

        [Test]
        public void ShouldClearDisplayWhenOFF()
        {
            _ra180.SendKey(Ra180KeyCode.ModKLAR);
            _synchronizationContext.Tick(Ra180.SELFTEST);
            _ra180.SendKey(Ra180KeyCode.Num4);
            _ra180.SendKey(Ra180KeyCode.ModFR);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("        "));
        }

        [Test]
        public void Should_not_restart_self_test_when_switching_between_KLAR_SKYDD_or_DRELÄ()
        {
            _ra180.SendKey(Ra180KeyCode.ModKLAR);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("TEST OK "));

            _ra180.SendKey(Ra180KeyCode.ModSKYDD);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("TEST OK "));

            _ra180.SendKey(Ra180KeyCode.ModDRELÄ);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("TEST OK "));
        }

        [Test]
        public void Should_not_start_new_self_test_when_already_on()
        {
            _ra180.SendKey(Ra180KeyCode.ModKLAR);
            _synchronizationContext.Tick(Ra180.SELFTEST);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("        "));

            _ra180.SendKey(Ra180KeyCode.ModSKYDD);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("        "), "KLAR->SKYDD");

            _ra180.SendKey(Ra180KeyCode.ModKLAR);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("        "), "SKYDD->KLAR");

            _ra180.SendKey(Ra180KeyCode.ModDRELÄ);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("        "), "KLAR->DRELÄ");

            _ra180.SendKey(Ra180KeyCode.ModKLAR);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("        "), "DRELÄ->KLAR");

            _ra180.SendKey(Ra180KeyCode.ModSKYDD);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("        "), "KLAR->SKYDD");

            _ra180.SendKey(Ra180KeyCode.ModDRELÄ);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("        "), "SKYDD->DRELÄ");

            _ra180.SendKey(Ra180KeyCode.ModSKYDD);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("        "), "DRELÄ->SKYDD");
        }

        [Test]
        public void Should_not_be_possible_to_modify_brightness_while_OFF()
        {
            var initialValue = _ra180.Display.Brightness;
            _ra180.SendKey(Ra180KeyCode.BEL);
            Assert.That(_ra180.Display.Brightness, Is.EqualTo(initialValue));
        }

        [Test]
        public void Should_be_possible_to_modify_brightness_during_self_test()
        {
            var initialValue = _ra180.Display.Brightness;
            _ra180.SendKey(Ra180KeyCode.ModKLAR);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            _ra180.SendKey(Ra180KeyCode.BEL);
            Assert.That(_ra180.Display.Brightness, Is.Not.EqualTo(initialValue));
        }

        [Test]
        public void Should_cycle_brightness()
        {
            _ra180.SendKey(Ra180KeyCode.ModKLAR);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);

            var values = new List<int>();
            var previousValue = _ra180.Display.Brightness;
            _ra180.SendKey(Ra180KeyCode.BEL);
            Assert.That(_ra180.Display.Brightness, Is.Not.EqualTo(previousValue), "#1");
            values.Add(previousValue);

            previousValue = _ra180.Display.Brightness;
            values.Add(_ra180.Display.Brightness);

            _ra180.SendKey(Ra180KeyCode.BEL);
            Assert.That(_ra180.Display.Brightness, Is.Not.EqualTo(previousValue), "#2");
            Assert.That(values, Has.No.Member(_ra180.Display.Brightness));

            previousValue = _ra180.Display.Brightness;
            values.Add(_ra180.Display.Brightness);

            _ra180.SendKey(Ra180KeyCode.BEL);
            Assert.That(_ra180.Display.Brightness, Is.Not.EqualTo(previousValue), "#3");
            Assert.That(values, Has.No.Member(_ra180.Display.Brightness));

            previousValue = _ra180.Display.Brightness;
            values.Add(_ra180.Display.Brightness);

            _ra180.SendKey(Ra180KeyCode.BEL);
            Assert.That(_ra180.Display.Brightness, Is.Not.EqualTo(previousValue), "#4");
            Assert.That(values, Has.No.Member(_ra180.Display.Brightness));

            previousValue = _ra180.Display.Brightness;
            values.Add(_ra180.Display.Brightness);

            _ra180.SendKey(Ra180KeyCode.BEL);
            Assert.That(_ra180.Display.Brightness, Is.Not.EqualTo(previousValue), "#5");
            Assert.That(values, Has.No.Member(_ra180.Display.Brightness));

            previousValue = _ra180.Display.Brightness;
            values.Add(_ra180.Display.Brightness);

            _ra180.SendKey(Ra180KeyCode.BEL);
            Assert.That(_ra180.Display.Brightness, Is.Not.EqualTo(previousValue), "#6");
            Assert.That(values, Has.Member(_ra180.Display.Brightness));
        }

        [Test]
        public void Should_not_reset_brightness()
        {
            _ra180.SendKey(Ra180KeyCode.ModKLAR);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            _ra180.SendKey(Ra180KeyCode.BEL);
            _ra180.SendKey(Ra180KeyCode.ModFR);
            var expected = _ra180.Display.Brightness;

            _ra180.SendKey(Ra180KeyCode.ModKLAR);

            Assert.That(_ra180.Display.Brightness, Is.EqualTo(expected));
        }
    }

    public class Helper
    {
        public static void EnterNewPny(Ra180 ra180)
        {
            throw new NotImplementedException();
        }
    }
}
