using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;

namespace Ra180.Tests
{
    [TestFixture]
    public class Ra180NykTests
    {
        private Ra180 _ra180;
        private DelayedSynchronizationContext _synchronizationContext;
        private Mock<IRa180Network> _network;
        private Ra180DataKey _key1;
        private Ra180DataKey _key2;

        [SetUp]
        public void SetUp()
        {
            _synchronizationContext = new DelayedSynchronizationContext();
            _network = new Mock<IRa180Network>();
            _ra180 = new Ra180(_network.Object, _synchronizationContext);

            _key1 = Ra180DataKey.Generate();
            _key2 = Ra180DataKey.Generate();

            _ra180.SendKey(Ra180Key.ModSKYDD);
            _synchronizationContext.Tick(Ra180.SELFTEST);
        }

        [Test]
        public void ShouldNotHaveActiveKeyByDefault()
        {
            _ra180.SendKey(Ra180Key.NYK);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("NYK:### "));
            _ra180.SendKey(Ra180Key.ÄND);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("NYK:### "));
        }

        [Test]
        public void WhenEnteredPassiveKey_ShouldSelectEnteredPNY()
        {
            EnterPNY(_key1);

            _ra180.SendKey(Ra180Key.NYK);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("NYK:### "));
            _ra180.SendKey(Ra180Key.ÄND);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo(string.Format("NYK:{0} ", _key1.Checksum)));
        }

        [Test]
        public void WhenEnteredPassiveKey()
        {
            EnterPNY(_key1);

            _ra180.SendKey(Ra180Key.NYK);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("NYK:### "));
            _ra180.SendKey(Ra180Key.ÄND);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo(string.Format("NYK:{0} ", _key1.Checksum)));
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.StringContaining("(NYK)"));
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("        "));
            _ra180.SendKey(Ra180Key.KDA);
            Assert.That(_ra180.Display.ToString(), Is.StringStarting("FR"));
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.StringStarting("BD1"));
        }

        private void EnterPNY(Ra180DataKey key)
        {
            EnterPNY(_ra180, key);
        }

        private static void EnterPNY(Ra180 ra180, Ra180DataKey key)
        {
            ra180.SendKey(Ra180Key.KDA); // FR
            ra180.SendKey(Ra180Key.ENT); // BD1
            ra180.SendKey(Ra180Key.ENT); // BD2/SYNK
            if (ra180.Display.ToString().StartsWith("BD2"))
                ra180.SendKey(Ra180Key.ENT); // SYNK
            ra180.SendKey(Ra180Key.ENT); // PNY
            ra180.SendKey(Ra180Key.ÄND);

            foreach (var group in key.Data)
            {
                ra180.SendKeys(group);
                ra180.SendKey(Ra180Key.ENT);
            }

            ra180.SendKey(Ra180Key.SLT);
        }
    }

    [TestFixture]
    public class Ra180KdaTests
    {
        private Ra180 _ra180;
        private DelayedSynchronizationContext _synchronizationContext;
        private Mock<IRa180Network> _network;

        [SetUp]
        public void SetUp()
        {
            _synchronizationContext = new DelayedSynchronizationContext();
            _network = new Mock<IRa180Network>();
            _ra180 = new Ra180(_network.Object, _synchronizationContext);

            _ra180.SendKey(Ra180Key.ModSKYDD);
            _synchronizationContext.Tick(Ra180.SELFTEST);
        }

        [Test]
        public void Should_hide_BD1_BD2_SYNK_and_PNY_while_in_KLAR()
        {
            _ra180.SendKey(Ra180Key.ModKLAR);
            _ra180.SendKey("4");
            Assert.That(_ra180.Display.ToString(), Is.StringMatching(@"^FR\:[0-9]{5}$"));
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("  (KDA) "));
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("        "));

        }

        [Test]
        public void Should_have_correct_standard_FR_values()
        {
            _ra180.SendKey("4");
            _ra180.SendKey(Ra180Key.Channel1);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("FR:30025"));
            _ra180.SendKey(Ra180Key.Channel2);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("FR:40025"));
            _ra180.SendKey(Ra180Key.Channel3);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("FR:50025"));
            _ra180.SendKey(Ra180Key.Channel4);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("FR:60025"));
            _ra180.SendKey(Ra180Key.Channel5);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("FR:70025"));
            _ra180.SendKey(Ra180Key.Channel6);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("FR:80025"));
            _ra180.SendKey(Ra180Key.Channel7);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("FR:87975"));
            _ra180.SendKey(Ra180Key.Channel8);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("FR:42025"));
        }

        [Test]
        public void Should_have_correct_standard_BD1_values()
        {
            _ra180.SendKey("4");
            _ra180.SendKey(Ra180Key.ENT);
            _ra180.SendKey(Ra180Key.Channel1);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("BD1:9000"));
            _ra180.SendKey(Ra180Key.Channel2);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("BD1:9000"));
            _ra180.SendKey(Ra180Key.Channel3);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("BD1:9000"));
            _ra180.SendKey(Ra180Key.Channel4);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("BD1:9000"));
            _ra180.SendKey(Ra180Key.Channel5);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("BD1:9000"));
            _ra180.SendKey(Ra180Key.Channel6);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("BD1:9000"));
            _ra180.SendKey(Ra180Key.Channel7);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("BD1:9000"));
            _ra180.SendKey(Ra180Key.Channel8);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("BD1:9000"));
        }

        [Test]
        public void should_be_possible_to_disable__and_reenable__KLAR()
        {
            _ra180.SendKey("4");
            _ra180.SendKey(Ra180Key.ÄND);
            _ra180.SendKey(Ra180Key.Asterix);
            _ra180.SendKey(Ra180Key.Asterix);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("FR:**   "));
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.StringMatching(@"^\*\*:[0-9]{5}$"));
            _ra180.SendKey(Ra180Key.SLT);

            _ra180.SendKey("4");
            Assert.That(_ra180.Display.ToString(), Is.StringMatching(@"^\*\*:[0-9]{5}$"));
            _ra180.SendKey(Ra180Key.ÄND);
            _ra180.SendKey(Ra180Key.Asterix);
            _ra180.SendKey(Ra180Key.Asterix);
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.StringMatching(@"^FR:[0-9]{5}$"));
            _ra180.SendKey(Ra180Key.SLT);
        }

        [Test]
        public void should_auto_reenable_KLAR_while_in_KLAR_mod()
        {
            _ra180.SendKey("4");
            _ra180.SendKey(Ra180Key.ÄND);
            _ra180.SendKey(Ra180Key.Asterix);
            _ra180.SendKey(Ra180Key.Asterix);
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.StringMatching(@"^\*\*:[0-9]{5}$"));
            _ra180.SendKey(Ra180Key.ModKLAR);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("**:00000"));
            _ra180.SendKey(Ra180Key.ModSKYDD);
            Assert.That(_ra180.Display.ToString(), Is.StringMatching(@"^\*\*:[0-9]{5}$"));
        }

        [Test]
        public void should_not_display_BD2_when_BD1_is_9000()
        {
            _ra180.SendKey("4");
            _ra180.SendKey(Ra180Key.ENT);
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.Not.StringMatching(@"^BD2:[0-9]{4}$"));
        }

        [Test]
        public void should_display_BD2_when_BD1_is_not_9000()
        {
            _ra180.SendKey("4");
            _ra180.SendKey(Ra180Key.ENT);
            _ra180.SendKey(Ra180Key.ÄND);
            _ra180.SendKeys("3040");
            _ra180.SendKey(Ra180Key.ENT);
            _ra180.SendKeys("5060");
            _ra180.SendKey(Ra180Key.ENT);
            _ra180.SendKey(Ra180Key.SLT);

            _ra180.SendKey("4");
            _ra180.SendKey(Ra180Key.ENT);
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.StringMatching(@"^BD2:[0-9]{4}$"));
        }

        [Test]
        public void should_navigate_KDA()
        {
            _ra180.SendKey("4");
            Assert.That(_ra180.Display.ToString(), Is.StringMatching(@"^FR:[0-9]{5}$"));
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.StringMatching(@"^BD1:[0-9]{4}$"));
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("SYNK=NEJ"));
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("PNY:### "));
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("  (KDA) "));
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("        "));
        }

        [Test]
        public void should_return_to_main_menu_on_SLT_from_FR()
        {
            _ra180.SendKey("4");
            Assert.That(_ra180.Display.ToString(), Is.StringMatching(@"^FR:[0-9]{5}$"));
            _ra180.SendKey(Ra180Key.SLT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("        "));
        }

        [Test]
        public void should_return_to_main_menu_on_SLT_from_BD1()
        {
            _ra180.SendKey("4");
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.StringMatching(@"^BD1:[0-9]{4}$"));
            _ra180.SendKey(Ra180Key.SLT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("        "));
        }

        [Test]
        public void should_return_to_main_menu_on_SLT_from_SYNK()
        {
            _ra180.SendKey("4");
            _ra180.SendKey(Ra180Key.ENT);
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("SYNK=NEJ"));
            _ra180.SendKey(Ra180Key.SLT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("        "));
        }

        [Test]
        public void should_return_to_main_menu_on_SLT_from_PNY()
        {
            _ra180.SendKey("4");
            _ra180.SendKey(Ra180Key.ENT);
            _ra180.SendKey(Ra180Key.ENT);
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("PNY:### "));
            _ra180.SendKey(Ra180Key.SLT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("        "));
        }

        [Test]
        public void should_return_to_main_menu_on_SLT_from_KDA()
        {
            _ra180.SendKey("4");
            _ra180.SendKey(Ra180Key.ENT);
            _ra180.SendKey(Ra180Key.ENT);
            _ra180.SendKey(Ra180Key.ENT);
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("  (KDA) "));
            _ra180.SendKey(Ra180Key.SLT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("        "));
        }

        [Test]
        public void should_allow_modification_of_SYNK_when_in_sync()
        {
            _network.Raise(m => m.ReceivedSynk += null, EventArgs.Empty);
            _ra180.SendKey("4");
            _ra180.SendKey(Ra180Key.ENT);
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("SYNK:JA "));
        }

        [Test]
        public void should_allow_edit_of_frequency()
        {
            _ra180.SendKey("4");
            _ra180.SendKey(Ra180Key.ÄND);
            _ra180.SendKey("42025");
            _ra180.SendKey(Ra180Key.ENT);
            _ra180.SendKey(Ra180Key.SLT);
            _ra180.SendKey("4");
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("FR:42025"));
        }

        [Test]
        public void should_reject_frequency_lower_than_30_000_MHz()
        {
            _ra180.SendKey("4");
            _ra180.SendKey(Ra180Key.ÄND);
            _ra180.SendKey("2");
            _ra180.SendKey("9");
            _ra180.SendKey("9");
            _ra180.SendKey("9");
            _ra180.SendKey("9");
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.Not.EqualTo("FR:29999"));
        }

        [Test]
        public void should_allow_frequency_equal_to_30_000_MHz()
        {
            _ra180.SendKey("4");
            _ra180.SendKey(Ra180Key.ÄND);
            _ra180.SendKey("3");
            _ra180.SendKey("0");
            _ra180.SendKey("0");
            _ra180.SendKey("0");
            _ra180.SendKey("0");
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("FR:30000"));
        }

        [Test]
        public void should_reject_frequency_higher_than_87_975_MHz()
        {
            _ra180.SendKey("4");
            _ra180.SendKey(Ra180Key.ÄND);
            _ra180.SendKey("8");
            _ra180.SendKey("7");
            _ra180.SendKey("9");
            _ra180.SendKey("7");
            _ra180.SendKey("6");
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.Not.EqualTo("FR:87976"));
        }

        public IEnumerable<TestCaseData> KanalseparationTests
        {
            get
            {
                for (var i = 30000; i <= 30100; i++)
                {
                    yield return new TestCaseData(i.ToString(), (i % 25) == 0);
                }
            }
        }

        [Test]
        [TestCaseSource("KanalseparationTests")]
        public void Kanalseparation25KHz(string frequency, bool expectsFrequencyAccepted)
        {
            _ra180.SendKey("4");
            _ra180.SendKey(Ra180Key.ÄND);
            _ra180.SendKey(frequency);
            _ra180.SendKey(Ra180Key.ENT);

            if (expectsFrequencyAccepted)
                Assert.That(_ra180.Display.ToString(), Is.EqualTo("FR:"+frequency));
            else
                Assert.That(_ra180.Display.ToString(), Is.EqualTo("FR:     "));
        }

        [Test]
        public void should_allow_frequency_equal_to_87_975_MHz()
        {
            _ra180.SendKey("4");
            _ra180.SendKey(Ra180Key.ÄND);
            _ra180.SendKey("8");
            _ra180.SendKey("7");
            _ra180.SendKey("9");
            _ra180.SendKey("7");
            _ra180.SendKey("5");
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("FR:87975"));
        }

        [Test]
        public void should_modify_BD2_after_BD1()
        {
            _ra180.SendKey("4");
            _ra180.SendKey(Ra180Key.ENT);
            _ra180.SendKey(Ra180Key.ÄND);
            _ra180.SendKey("4");
            _ra180.SendKey("5");
            _ra180.SendKey("5");
            _ra180.SendKey("5");
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("BD1:4555"));
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("BD2:    "));
            _ra180.SendKey("6");
            _ra180.SendKey("5");
            _ra180.SendKey("7");
            _ra180.SendKey("5");
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("BD2:6575"));
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("BD1:4555"));
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("BD2:6575"));
        }

        [Test]
        public void InUrkoppling_av_KLAR_funktion_ModKLAR()
        {
            _ra180.Mod = Ra180Mod.KLAR;
            _ra180.SendKey("4");
            Assert.That(_ra180.Display.ToString(), Is.StringMatching(@"^FR:[0-9]{5}$"));
            _ra180.SendKey(Ra180Key.ÄND);
            _ra180.SendKeys("**");
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("**:00000"));
            _ra180.SendKey(Ra180Key.ÄND);
            _ra180.SendKeys("**");
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.StringMatching(@"^FR:[0-9]{5}$"));
        }

        [Test]
        public void InUrkoppling_av_KLAR_funktion_ModSKYDD()
        {
            _ra180.Mod = Ra180Mod.SKYDD;
            _ra180.SendKey("4");
            Assert.That(_ra180.Display.ToString(), Is.StringMatching(@"^FR:[0-9]{5}$"));
            _ra180.SendKey(Ra180Key.ÄND);
            _ra180.SendKeys("**");
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.StringMatching(@"^\*\*:[0-9]{5}$"));
            _ra180.SendKey(Ra180Key.ÄND);
            _ra180.SendKeys("**");
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.StringMatching(@"^FR:[0-9]{5}$"));
        }

        [Test]
        public void should_modify_BD1_on_AND_for_BD2()
        {
            _ra180.SendKey("4");
            _ra180.SendKey(Ra180Key.ENT);
            _ra180.SendKey(Ra180Key.ÄND);
            _ra180.SendKeys("3040");
            _ra180.SendKey(Ra180Key.ENT);
            _ra180.SendKeys("5060");
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo(@"BD1:3040"));
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo(@"BD2:5060"));
            _ra180.SendKey(Ra180Key.ÄND);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("BD1:    "));
        }

        [Test]
        public void should_modify_PNY_via_PN1_8()
        {
            _ra180.SendKey("4"); // FR
            Assert.That(_ra180.Display.ToString(), Is.StringMatching("^FR:"));
            _ra180.SendKey(Ra180Key.ENT); // BD1
            Assert.That(_ra180.Display.ToString(), Is.StringMatching("^BD1:"));
            _ra180.SendKey(Ra180Key.ENT); // SYNK
            Assert.That(_ra180.Display.ToString(), Is.StringMatching("^SYNK"));
            _ra180.SendKey(Ra180Key.ENT); // PNY=###
            Assert.That(_ra180.Display.ToString(), Is.StringMatching("^PNY"));
            _ra180.SendKey(Ra180Key.ÄND);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("PN1:    "));
            _ra180.SendKeys("4422");
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("PN2:    "));
            _ra180.SendKeys("2211");
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("PN3:    "));
            _ra180.SendKeys("3300");
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("PN4:    "));
            _ra180.SendKeys("5511");
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("PN5:    "));
            _ra180.SendKeys("4325");
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("PN6:    "));
            _ra180.SendKeys("5621");
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("PN7:    "));
            _ra180.SendKeys("3201");
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("PN8:    "));
            _ra180.SendKeys("5104");
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.StringMatching(@"^PNY:[0-9]{3} $"));
            _ra180.SendKey(Ra180Key.SLT);
            _ra180.SendKey("4"); // FR
            Assert.That(_ra180.Display.ToString(), Is.StringMatching("^FR:"));
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.StringMatching("^BD1:"));
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.StringMatching("^SYNK"));
            _ra180.SendKey(Ra180Key.ENT);
            Assert.That(_ra180.Display.ToString(), Is.StringMatching(@"^PNY:[0-9]{3} $"));
        }
    }
}