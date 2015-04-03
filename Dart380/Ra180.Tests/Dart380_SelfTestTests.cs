using System.Collections.Generic;
using NUnit.Framework;
using Ra180.Devices.Dart380;

namespace Ra180.Tests
{
    [TestFixture]
    public class Dart380_SelfTestTests
    {
        private Dart380 _ra180;
        private DelayedSynchronizationContext _synchronizationContext;

        [SetUp]
        public void SetUp()
        {
            _synchronizationContext = new DelayedSynchronizationContext();
            _ra180 = new Dart380(_synchronizationContext);
        }

        [Test]
        public void ShouldPerformSelfTestOnKLAR()
        {
            _ra180.SendKey(Ra180Key.ModKLAR);
            Assert.That(_ra180.SmallDisplay.ToString(), Is.EqualTo("TEST    "));
            Assert.That(_ra180.LargeDisplay.ToString(), Is.EqualTo("                "));

            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            Assert.That(_ra180.SmallDisplay.ToString(), Is.EqualTo("TEST OK "));
            Assert.That(_ra180.LargeDisplay.ToString(), Is.EqualTo("                "));

            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            Assert.That(_ra180.SmallDisplay.ToString(), Is.EqualTo("NOLLST  "));
            Assert.That(_ra180.LargeDisplay.ToString(), Is.EqualTo("                "));

            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            Assert.That(_ra180.SmallDisplay.ToString(), Is.EqualTo("        "));
            Assert.That(_ra180.LargeDisplay.ToString(), Is.EqualTo("                "));
        }

        [Test]
        public void ShouldNotDelayChangeOfModToKLAR()
        {
            _ra180.SendKey(Ra180Key.ModKLAR);
            Assert.That(_ra180.Mod, Is.EqualTo(Dart380Mod.KLAR));
        }

        [Test]
        public void ShouldNotDelayChangeOfModToSKYDD()
        {
            _ra180.SendKey(Ra180Key.ModSKYDD);
            Assert.That(_ra180.Mod, Is.EqualTo(Dart380Mod.SKYDD));
        }

        [Test]
        public void ShouldNotDelayChangeOfModToDRELÄ()
        {
            _ra180.SendKey(Ra180Key.ModDRELÄ);
            Assert.That(_ra180.Mod, Is.EqualTo(Dart380Mod.DRELÄ));
        }

        [Test]
        public void ShouldNotAcceptInputUntilSelfTestIsComplete()
        {
            _ra180.SendKey(Ra180Key.ModKLAR);
            _ra180.SendKey(Ra180Key.Num1);
            Assert.That(_ra180.SmallDisplay, Is.Not.StringMatching("^T:"));
        }

        [Test]
        public void ShouldNotDisplayNOLLSTIfEnteredKDA()
        {
            _ra180.SendKey(Ra180Key.ModSKYDD);
            _synchronizationContext.Tick(Ra180.SELFTEST);
            Helper.EnterNewPny(_ra180);

            _ra180.SendKey(Ra180Key.ModFRÅN);
            _ra180.SendKey(Ra180Key.ModSKYDD);

            Assert.That(_ra180.SmallDisplay.ToString(), Is.EqualTo("TEST    "));

            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            Assert.That(_ra180.SmallDisplay.ToString(), Is.EqualTo("TEST OK "));

            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            Assert.That(_ra180.SmallDisplay.ToString(), Is.EqualTo("        "));
        }

        [Test]
        public void ShouldPerformSelfTestAfterReset()
        {
            _ra180.SendKey(Ra180Key.ModSKYDD);
            _synchronizationContext.Tick(Ra180.SELFTEST);

            _ra180.SendKeys(Ra180Key.Asterix, Ra180Key.NumberSign);
            Assert.That(_ra180.SmallDisplay.ToString(), Is.EqualTo("TEST    "));

            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            Assert.That(_ra180.SmallDisplay.ToString(), Is.EqualTo("TEST OK "));

            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            Assert.That(_ra180.SmallDisplay.ToString(), Is.EqualTo("NOLLST  "));

            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            Assert.That(_ra180.SmallDisplay.ToString(), Is.EqualTo("        "));
        }

        [Test]
        public void ShouldCancelSelfTestFromTESTWhenOFF()
        {
            _ra180.SendKey(Ra180Key.ModKLAR);
            _ra180.SendKey(Ra180Key.ModFRÅN);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            Assert.That(_ra180.SmallDisplay.ToString(), Is.EqualTo("        "));
        }

        [Test]
        public void ShouldCancelSelfTestFromTESTOKWhenOFF()
        {
            _ra180.SendKey(Ra180Key.ModKLAR);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            _ra180.SendKey(Ra180Key.ModFRÅN);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            Assert.That(_ra180.SmallDisplay.ToString(), Is.EqualTo("        "));
        }

        [Test]
        public void ShouldCancelSelfTestFromNOLLSTWhenOFF()
        {
            _ra180.SendKey(Ra180Key.ModKLAR);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            _ra180.SendKey(Ra180Key.ModFRÅN);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            Assert.That(_ra180.SmallDisplay.ToString(), Is.EqualTo("        "));
        }

        [Test]
        public void ShouldClearDisplayWhenOFF()
        {
            _ra180.SendKey(Ra180Key.ModKLAR);
            _synchronizationContext.Tick(Ra180.SELFTEST);
            _ra180.SendKey(Ra180Key.Num4);
            _ra180.SendKey(Ra180Key.ModFRÅN);
            Assert.That(_ra180.SmallDisplay.ToString(), Is.EqualTo("        "));
        }

        [Test]
        public void Should_not_restart_self_test_when_switching_between_KLAR_SKYDD_or_DRELÄ()
        {
            _ra180.SendKey(Ra180Key.ModKLAR);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            Assert.That(_ra180.SmallDisplay.ToString(), Is.EqualTo("TEST OK "));

            _ra180.SendKey(Ra180Key.ModSKYDD);
            Assert.That(_ra180.SmallDisplay.ToString(), Is.EqualTo("TEST OK "));

            _ra180.SendKey(Ra180Key.ModDRELÄ);
            Assert.That(_ra180.SmallDisplay.ToString(), Is.EqualTo("TEST OK "));
        }

        [Test]
        public void Should_not_start_new_self_test_when_already_on()
        {
            _ra180.SendKey(Ra180Key.ModKLAR);
            _synchronizationContext.Tick(Ra180.SELFTEST);
            Assert.That(_ra180.SmallDisplay.ToString(), Is.EqualTo("        "));

            _ra180.SendKey(Ra180Key.ModSKYDD);
            Assert.That(_ra180.SmallDisplay.ToString(), Is.EqualTo("        "), "KLAR->SKYDD");

            _ra180.SendKey(Ra180Key.ModKLAR);
            Assert.That(_ra180.SmallDisplay.ToString(), Is.EqualTo("        "), "SKYDD->KLAR");

            _ra180.SendKey(Ra180Key.ModDRELÄ);
            Assert.That(_ra180.SmallDisplay.ToString(), Is.EqualTo("        "), "KLAR->DRELÄ");

            _ra180.SendKey(Ra180Key.ModKLAR);
            Assert.That(_ra180.SmallDisplay.ToString(), Is.EqualTo("        "), "DRELÄ->KLAR");

            _ra180.SendKey(Ra180Key.ModSKYDD);
            Assert.That(_ra180.SmallDisplay.ToString(), Is.EqualTo("        "), "KLAR->SKYDD");

            _ra180.SendKey(Ra180Key.ModDRELÄ);
            Assert.That(_ra180.SmallDisplay.ToString(), Is.EqualTo("        "), "SKYDD->DRELÄ");

            _ra180.SendKey(Ra180Key.ModSKYDD);
            Assert.That(_ra180.SmallDisplay.ToString(), Is.EqualTo("        "), "DRELÄ->SKYDD");
        }

        [Test]
        public void Should_not_be_possible_to_modify_brightness_while_OFF()
        {
            var initialValue = _ra180.SmallDisplay.Brightness;
            _ra180.SendKey(Ra180Key.BEL);
            Assert.That(_ra180.SmallDisplay.Brightness, Is.EqualTo(initialValue));
        }

        [Test]
        public void Should_be_possible_to_modify_brightness_during_self_test()
        {
            var initialValue = _ra180.SmallDisplay.Brightness;
            _ra180.SendKey(Ra180Key.ModKLAR);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            _ra180.SendKey(Ra180Key.BEL);
            Assert.That(_ra180.SmallDisplay.Brightness, Is.Not.EqualTo(initialValue));
        }

        [Test]
        public void Should_cycle_brightness()
        {
            _ra180.SendKey(Ra180Key.ModKLAR);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);

            var values = new List<int>();
            var previousValue = _ra180.SmallDisplay.Brightness;
            _ra180.SendKey(Ra180Key.BEL);
            Assert.That(_ra180.SmallDisplay.Brightness, Is.Not.EqualTo(previousValue), "#1");
            values.Add(previousValue);

            previousValue = _ra180.SmallDisplay.Brightness;
            values.Add(_ra180.SmallDisplay.Brightness);

            _ra180.SendKey(Ra180Key.BEL);
            Assert.That(_ra180.SmallDisplay.Brightness, Is.Not.EqualTo(previousValue), "#2");
            Assert.That(values, Has.No.Member(_ra180.SmallDisplay.Brightness));

            previousValue = _ra180.SmallDisplay.Brightness;
            values.Add(_ra180.SmallDisplay.Brightness);

            _ra180.SendKey(Ra180Key.BEL);
            Assert.That(_ra180.SmallDisplay.Brightness, Is.Not.EqualTo(previousValue), "#3");
            Assert.That(values, Has.No.Member(_ra180.SmallDisplay.Brightness));

            previousValue = _ra180.SmallDisplay.Brightness;
            values.Add(_ra180.SmallDisplay.Brightness);

            _ra180.SendKey(Ra180Key.BEL);
            Assert.That(_ra180.SmallDisplay.Brightness, Is.Not.EqualTo(previousValue), "#4");
            Assert.That(values, Has.No.Member(_ra180.SmallDisplay.Brightness));

            previousValue = _ra180.SmallDisplay.Brightness;
            values.Add(_ra180.SmallDisplay.Brightness);

            _ra180.SendKey(Ra180Key.BEL);
            Assert.That(_ra180.SmallDisplay.Brightness, Is.Not.EqualTo(previousValue), "#5");
            Assert.That(values, Has.No.Member(_ra180.SmallDisplay.Brightness));

            previousValue = _ra180.SmallDisplay.Brightness;
            values.Add(_ra180.SmallDisplay.Brightness);

            _ra180.SendKey(Ra180Key.BEL);
            Assert.That(_ra180.SmallDisplay.Brightness, Is.Not.EqualTo(previousValue), "#6");
            Assert.That(values, Has.Member(_ra180.SmallDisplay.Brightness));
        }

        [Test]
        public void Should_not_reset_brightness()
        {
            _ra180.SendKey(Ra180Key.ModKLAR);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            _ra180.SendKey(Ra180Key.BEL);
            _ra180.SendKey(Ra180Key.ModFRÅN);
            var expected = _ra180.SmallDisplay.Brightness;

            _ra180.SendKey(Ra180Key.ModKLAR);

            Assert.That(_ra180.SmallDisplay.Brightness, Is.EqualTo(expected));
        }
    }
}