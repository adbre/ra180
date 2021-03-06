﻿using System;
using C42A.Ra180.Infrastructure;
using NUnit.Framework;

namespace C42A.Ra180.Tests
{
    public class MockDartTransport : IDartTransport
    {
        public Action<string> SendingString { get; set; }

        public void OnReceivedString(string s)
        {
            var handler = ReceivedString;
            if (handler != null) handler(this, s);
        }

        public event EventHandler<string> ReceivedString;

        public void SendString(string s)
        {
            var sending = SendingString;
            if (sending != null) sending(s);
        }
    }

    [TestFixture]
    public class Ra180UnitTests
    {
        private MockDartTransport _dartTransport;

        [SetUp]
        public void SetUp()
        {
            _dartTransport = new MockDartTransport();
        }

        private Ra180Unit GetSystemUnderTest()
        {
            var taskFactory = new DummyTaskFactory();
            return new Ra180Unit(taskFactory, _dartTransport);
        }

        [Test]
        public void ShouldReceiveMessageInKlar()
        {
            const string expected = "99FB75D9-C4B1-42D7-B98B-0243C19FD366";
            var sut = GetSystemUnderTest();
            sut.SendKeys(Ra180Knapp.ModKlar);

            _dartTransport.SendingString = s => _dartTransport.OnReceivedString(s);

            var actual = default(string);
            sut.ReceivedString += (sender, s) => actual = s;
            sut.SendString(expected);
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void ShouldBeFrånByDefault()
        {
            var sut = GetSystemUnderTest();
            var actual = sut.Mod;

            Assert.That(actual, Is.EqualTo(Ra180Mod.Från), "#1");
        }

        [Test]
        public void ShouldBeKanal1ByDefault()
        {
            var sut = GetSystemUnderTest();
            var actual = sut.Kanal;

            Assert.That(actual, Is.EqualTo(1), "#1");
        }

        [Test]
        public void ShouldBeVolym4ByDefault()
        {
            var sut = GetSystemUnderTest();
            Assert.That(sut.Volym, Is.EqualTo(4), "#1");
        }

        [Test]
        [TestCase(Ra180Knapp.ModFrån, Ra180Mod.Från)]
        [TestCase(Ra180Knapp.ModKlar, Ra180Mod.Klar)]
        [TestCase(Ra180Knapp.ModSkydd, Ra180Mod.Skydd)]
        [TestCase(Ra180Knapp.ModDRelä, Ra180Mod.DRelä)]
        public void ShouldSetModToKlarOnKeyKlar(Ra180Knapp knapp, Ra180Mod expected)
        {
            var sut = GetSystemUnderTest();
            sut.SendKeys(knapp);

            Assert.That(sut.Mod, Is.EqualTo(expected), "#1");
        }

        [Test]
        [TestCase(Ra180Knapp.Kanal1, 1)]
        [TestCase(Ra180Knapp.Kanal2, 2)]
        [TestCase(Ra180Knapp.Kanal3, 3)]
        [TestCase(Ra180Knapp.Kanal4, 4)]
        [TestCase(Ra180Knapp.Kanal5, 5)]
        [TestCase(Ra180Knapp.Kanal6, 6)]
        [TestCase(Ra180Knapp.Kanal7, 7)]
        [TestCase(Ra180Knapp.Kanal8, 8)]
        public void ShouldChangeKanal(Ra180Knapp knapp, int expected)
        {
            var sut = GetSystemUnderTest();
            sut.SendKeys(knapp);

            Assert.That(sut.Kanal, Is.EqualTo(expected), "#1");
        }

        [Test]
        [TestCase(Ra180Knapp.Volym1, 1)]
        [TestCase(Ra180Knapp.Volym2, 2)]
        [TestCase(Ra180Knapp.Volym3, 3)]
        [TestCase(Ra180Knapp.Volym4, 4)]
        [TestCase(Ra180Knapp.Volym5, 5)]
        [TestCase(Ra180Knapp.Volym6, 6)]
        [TestCase(Ra180Knapp.Volym7, 7)]
        [TestCase(Ra180Knapp.Volym8, 8)]
        public void ShouldChangeVolym(Ra180Knapp knapp, int expected)
        {
            var sut = GetSystemUnderTest();
            sut.SendKeys(knapp);

            Assert.That(sut.Volym, Is.EqualTo(expected), "#1");
        }

        [Test]
        public void ShouldDisplayTime()
        {
            var sut = GetSystemUnderTest();
            sut.SendKeys(Ra180Knapp.ModKlar);
            sut.SendKeys(Ra180Knapp.Knapp1);

            Assert.That(sut.Display, Is.EqualTo("T:000000"), "#1");
        }

        [Test]
        public void ShouldNotDisplayTimeWhenFrån()
        {
            var sut = GetSystemUnderTest();
            sut.SendKeys(Ra180Knapp.Knapp1);

            Assert.That(sut.Display, Is.EqualTo(null), "#1");
        }

        [Test]
        public void ShouldSetDateAndTime()
        {
            var sut = GetSystemUnderTest();
            sut.SendKeys(Ra180Knapp.ModKlar);
            sut.SendKeys(Ra180Knapp.Knapp1);
            sut.SendKeys(Ra180Knapp.ÄND);
            Assert.That(sut.Display, Is.EqualTo("T:"), "#1");

            sut.SendKeys(Ra180Knapp.Knapp2);
            sut.SendKeys(Ra180Knapp.Knapp1);
            sut.SendKeys(Ra180Knapp.Knapp5);
            sut.SendKeys(Ra180Knapp.Knapp0);
            sut.SendKeys(Ra180Knapp.Knapp3);
            sut.SendKeys(Ra180Knapp.Knapp6);
            Assert.That(sut.Display, Is.EqualTo("T:215036"), "#2");

            sut.SendKeys(Ra180Knapp.ENT);
            Assert.That(sut.Display, Is.EqualTo("T:215036"), "#3");

            sut.SendKeys(Ra180Knapp.ENT);
            Assert.That(sut.Display, Is.EqualTo("DAT:0101"), "#4");
            sut.SendKeys(Ra180Knapp.ÄND);
            Assert.That(sut.Display, Is.EqualTo("DAT:"), "#5");
            sut.SendKeys(Ra180Knapp.Knapp0);
            sut.SendKeys(Ra180Knapp.Knapp1);
            sut.SendKeys(Ra180Knapp.Knapp2);
            sut.SendKeys(Ra180Knapp.Knapp9);
            Assert.That(sut.Display, Is.EqualTo("DAT:0129"), "#6");

            sut.SendKeys(Ra180Knapp.ENT);
            Assert.That(sut.Display, Is.EqualTo("DAT:0129"), "#7");

            sut.SendKeys(Ra180Knapp.ENT);
            Assert.That(sut.Display, Is.EqualTo("  (TID) "), "#8");

            sut.SendKeys(Ra180Knapp.ENT);
            Assert.That(sut.Display, Is.EqualTo(null), "#9");

            sut.SendKeys(Ra180Knapp.Knapp1);
            Assert.That(sut.Display, Is.EqualTo("T:215036"), "#10");
            sut.SendKeys(Ra180Knapp.ENT);
            Assert.That(sut.Display, Is.EqualTo("DAT:0129"), "#11");
            sut.SendKeys(Ra180Knapp.SLT);
            Assert.That(sut.Display, Is.EqualTo(null), "#12");
        }

        [Test]
        public void ShouldAbortTidInput()
        {
            var sut = GetSystemUnderTest();
            sut.SendKeys(Ra180Knapp.ModKlar);
            sut.SendKeys(Ra180Knapp.Knapp1);
            sut.SendKeys(Ra180Knapp.ÄND);
            Assert.That(sut.Display, Is.EqualTo("T:"), "#1");

            sut.SendKeys(Ra180Knapp.Knapp2);
            sut.SendKeys(Ra180Knapp.Knapp1);
            sut.SendKeys(Ra180Knapp.Knapp5);
            sut.SendKeys(Ra180Knapp.SLT);
            Assert.That(sut.Display, Is.EqualTo("T:000000"), "#2");

            sut.SendKeys(Ra180Knapp.SLT);
            Assert.That(sut.Display, Is.EqualTo(null), "#3");
        }

        [Test]
        public void ShouldNavigateRda()
        {
            var sut = GetSystemUnderTest();
            sut.SendKeys(Ra180Knapp.ModKlar);
            sut.SendKeys(Ra180Knapp.Knapp2);
            Assert.That(sut.Display, Is.EqualTo("SDX=NEJ"), "#1");

            sut.SendKeys(Ra180Knapp.ENT);
            Assert.That(sut.Display, Is.EqualTo("OPMTN=JA"), "#2");

            sut.SendKeys(Ra180Knapp.ENT);
            Assert.That(sut.Display, Is.EqualTo("BAT:12.5"), "#3");

            sut.SendKeys(Ra180Knapp.ENT);
            Assert.That(sut.Display, Is.EqualTo("  (RDA) "), "#4");

            sut.SendKeys(Ra180Knapp.ENT);
            Assert.That(sut.Display, Is.EqualTo(null), "#5");
        }

        [Test]
        public void ShouldNotAllowEditRda()
        {
            var sut = GetSystemUnderTest();
            sut.SendKeys(Ra180Knapp.ModKlar);
            sut.SendKeys(Ra180Knapp.Knapp2);
            sut.SendKeys(Ra180Knapp.ENT);
            sut.SendKeys(Ra180Knapp.ENT);
            Assert.That(sut.Display, Is.EqualTo("BAT:12.5"), "#1");

            sut.SendKeys(Ra180Knapp.ÄND);
            sut.SendKeys(Ra180Knapp.ENT);
            Assert.That(sut.Display, Is.EqualTo("  (RDA) "), "#2");
        }

        [Test]
        public void ShouldNavigateKda()
        {
            var sut = GetSystemUnderTest();
            sut.SendKeys(Ra180Knapp.ModKlar);
            sut.SendKeys(Ra180Knapp.Knapp4);
            Assert.That(sut.Display, Is.EqualTo("FR:30060"), "#1");

            sut.SendKeys(Ra180Knapp.ENT);
            Assert.That(sut.Display, Is.EqualTo("BD1:1234"), "#2");

            sut.SendKeys(Ra180Knapp.ENT);
            Assert.That(sut.Display, Is.EqualTo("BD2:5678"), "#3");

            sut.SendKeys(Ra180Knapp.ENT);
            Assert.That(sut.Display, Is.EqualTo("SYNK=NEJ"), "#4");

            sut.SendKeys(Ra180Knapp.ENT);
            Assert.That(sut.Display, Is.EqualTo("PNY:###"), "#5");

            sut.SendKeys(Ra180Knapp.ENT);
            Assert.That(sut.Display, Is.EqualTo("  (KDA) "), "#6");

            sut.SendKeys(Ra180Knapp.ENT);
            Assert.That(sut.Display, Is.EqualTo(null), "#7");
        }

        [Test]
        public void ShouldEnterFrekvensKanal1()
        {
            var sut = GetSystemUnderTest();
            sut.SendKeys(Ra180Knapp.ModKlar);
            sut.SendKeys(Ra180Knapp.Kanal1);
            sut.SendKeys(Ra180Knapp.Knapp4);
            sut.SendKeys(Ra180Knapp.ÄND);
            Assert.That(sut.Display, Is.EqualTo("FR:"), "#1");

            sut.SendKeys(Ra180Knapp.Knapp1);
            sut.SendKeys(Ra180Knapp.Knapp2);
            sut.SendKeys(Ra180Knapp.Knapp3);
            sut.SendKeys(Ra180Knapp.Knapp4);
            sut.SendKeys(Ra180Knapp.Knapp5);
            Assert.That(sut.Display, Is.EqualTo("FR:12345"), "#2");

            sut.SendKeys(Ra180Knapp.ENT);
            Assert.That(sut.Display, Is.EqualTo("FR:12345"), "#4");

            sut.SendKeys(Ra180Knapp.SLT);
            Assert.That(sut.Display, Is.EqualTo(null), "#5");

            sut.SendKeys(Ra180Knapp.Knapp4);
            Assert.That(sut.Display, Is.EqualTo("FR:12345"), "#6");
        }

        [Test]
        public void ShouldNotAllowMoreThan8Characters()
        {
            var sut = GetSystemUnderTest();
            sut.SendKeys(Ra180Knapp.ModKlar);
            sut.SendKeys(Ra180Knapp.Knapp1);
            sut.SendKeys(Ra180Knapp.ÄND);
            sut.SendKeys(Ra180Knapp.Knapp1);
            sut.SendKeys(Ra180Knapp.Knapp2);
            sut.SendKeys(Ra180Knapp.Knapp3);
            sut.SendKeys(Ra180Knapp.Knapp4);
            sut.SendKeys(Ra180Knapp.Knapp5);
            sut.SendKeys(Ra180Knapp.Knapp6);
            sut.SendKeys(Ra180Knapp.Knapp7);
            Assert.That(sut.Display, Is.EqualTo("T:123456"), "#2");
        }

        [Test]
        public void ShouldNotAllowMoreThan5CharactersFrekvens()
        {
            var sut = GetSystemUnderTest();
            sut.SendKeys(Ra180Knapp.ModKlar);
            sut.SendKeys(Ra180Knapp.Knapp4);
            sut.SendKeys(Ra180Knapp.ÄND);
            sut.SendKeys(Ra180Knapp.Knapp1);
            sut.SendKeys(Ra180Knapp.Knapp2);
            sut.SendKeys(Ra180Knapp.Knapp3);
            sut.SendKeys(Ra180Knapp.Knapp4);
            sut.SendKeys(Ra180Knapp.Knapp5);
            sut.SendKeys(Ra180Knapp.Knapp6);
            Assert.That(sut.Display, Is.EqualTo("FR:12345"), "#2");
        }

        [Test]
        public void ShouldEnterBandbredd1And2()
        {
            var sut = GetSystemUnderTest();
            sut.SendKeys(Ra180Knapp.ModKlar);
            sut.SendKeys(Ra180Knapp.Knapp4);
            sut.SendKeys(Ra180Knapp.ENT);
            sut.SendKeys(Ra180Knapp.ÄND);
            sut.SendKeys(Ra180Knapp.Knapp3);
            sut.SendKeys(Ra180Knapp.Knapp0);
            sut.SendKeys(Ra180Knapp.Knapp4);
            sut.SendKeys(Ra180Knapp.Knapp0);
            sut.SendKeys(Ra180Knapp.ENT);
            sut.SendKeys(Ra180Knapp.Knapp5);
            sut.SendKeys(Ra180Knapp.Knapp0);
            sut.SendKeys(Ra180Knapp.Knapp6);
            sut.SendKeys(Ra180Knapp.Knapp0);
            sut.SendKeys(Ra180Knapp.ENT);
            Assert.That(sut.Display, Is.EqualTo("BD1:3040"), "#1");
            sut.SendKeys(Ra180Knapp.SLT);
            sut.SendKeys(Ra180Knapp.Knapp4);
            sut.SendKeys(Ra180Knapp.ENT);
            Assert.That(sut.Display, Is.EqualTo("BD1:3040"), "#2");
            sut.SendKeys(Ra180Knapp.ENT);
            Assert.That(sut.Display, Is.EqualTo("BD2:5060"), "#3");
        }

        [Test]
        public void ShouldEnterPnyGroups()
        {
            var sut = GetSystemUnderTest();
            sut.SendKeys(Ra180Knapp.ModKlar);
            sut.SendKeys(Ra180Knapp.Knapp4);
            sut.SendKeys(Ra180Knapp.ENT); // FR->BD1
            sut.SendKeys(Ra180Knapp.ENT); // BD1->BD2
            sut.SendKeys(Ra180Knapp.ENT); // BD2->SYNK
            sut.SendKeys(Ra180Knapp.ENT); // SYNK->PNY
            sut.SendKeys(Ra180Knapp.ÄND);
            Assert.That(sut.Display, Is.EqualTo("PN1:"), "#1");
            sut.SendKeys(Ra180Knapp.Knapp1);
            sut.SendKeys(Ra180Knapp.Knapp1);
            sut.SendKeys(Ra180Knapp.Knapp1);
            sut.SendKeys(Ra180Knapp.Knapp1);
            Assert.That(sut.Display, Is.EqualTo("PN1:1111"), "#2");
            sut.SendKeys(Ra180Knapp.ENT);
            Assert.That(sut.Display, Is.EqualTo("PN2:"), "#3");
            sut.SendKeys(Ra180Knapp.Knapp2);
            sut.SendKeys(Ra180Knapp.Knapp2);
            sut.SendKeys(Ra180Knapp.Knapp2);
            sut.SendKeys(Ra180Knapp.Knapp2);
            Assert.That(sut.Display, Is.EqualTo("PN2:2222"), "#4");
            sut.SendKeys(Ra180Knapp.ENT);
            Assert.That(sut.Display, Is.EqualTo("PN3:"), "#5");
            sut.SendKeys(Ra180Knapp.Knapp3);
            sut.SendKeys(Ra180Knapp.Knapp3);
            sut.SendKeys(Ra180Knapp.Knapp3);
            sut.SendKeys(Ra180Knapp.Knapp3);
            Assert.That(sut.Display, Is.EqualTo("PN3:3333"), "#6");
            sut.SendKeys(Ra180Knapp.ENT);
            Assert.That(sut.Display, Is.EqualTo("PN4:"), "#7");
            sut.SendKeys(Ra180Knapp.Knapp4);
            sut.SendKeys(Ra180Knapp.Knapp4);
            sut.SendKeys(Ra180Knapp.Knapp4);
            sut.SendKeys(Ra180Knapp.Knapp4);
            Assert.That(sut.Display, Is.EqualTo("PN4:4444"), "#8");
            sut.SendKeys(Ra180Knapp.ENT);
            Assert.That(sut.Display, Is.EqualTo("PN5:"), "#9");
            sut.SendKeys(Ra180Knapp.Knapp5);
            sut.SendKeys(Ra180Knapp.Knapp5);
            sut.SendKeys(Ra180Knapp.Knapp5);
            sut.SendKeys(Ra180Knapp.Knapp5);
            Assert.That(sut.Display, Is.EqualTo("PN5:5555"), "#10");
            sut.SendKeys(Ra180Knapp.ENT);
            Assert.That(sut.Display, Is.EqualTo("PN6:"), "#11");
            sut.SendKeys(Ra180Knapp.Knapp6);
            sut.SendKeys(Ra180Knapp.Knapp6);
            sut.SendKeys(Ra180Knapp.Knapp6);
            sut.SendKeys(Ra180Knapp.Knapp6);
            Assert.That(sut.Display, Is.EqualTo("PN6:6666"), "#12");
            sut.SendKeys(Ra180Knapp.ENT);
            Assert.That(sut.Display, Is.EqualTo("PN7:"), "#13");
            sut.SendKeys(Ra180Knapp.Knapp7);
            sut.SendKeys(Ra180Knapp.Knapp7);
            sut.SendKeys(Ra180Knapp.Knapp7);
            sut.SendKeys(Ra180Knapp.Knapp7);
            Assert.That(sut.Display, Is.EqualTo("PN7:7777"), "#14");
            sut.SendKeys(Ra180Knapp.ENT);
            Assert.That(sut.Display, Is.EqualTo("PN8:"), "#15");
            sut.SendKeys(Ra180Knapp.Knapp8);
            sut.SendKeys(Ra180Knapp.Knapp8);
            sut.SendKeys(Ra180Knapp.Knapp8);
            sut.SendKeys(Ra180Knapp.Knapp8);
            Assert.That(sut.Display, Is.EqualTo("PN8:8888"), "#16");
            sut.SendKeys(Ra180Knapp.ENT);
            Assert.That(sut.Display, Is.EqualTo("PN9:"), "#17");
            sut.SendKeys(Ra180Knapp.Knapp9);
            sut.SendKeys(Ra180Knapp.Knapp9);
            sut.SendKeys(Ra180Knapp.Knapp9);
            sut.SendKeys(Ra180Knapp.Knapp9);
            Assert.That(sut.Display, Is.EqualTo("PN9:9999"), "#18");
            sut.SendKeys(Ra180Knapp.ENT);
            Assert.That(sut.Display, Is.EqualTo("PNY:111"), "#19");
            sut.SendKeys(Ra180Knapp.SLT);
            sut.SendKeys(Ra180Knapp.Knapp7);
            Assert.That(sut.Display, Is.EqualTo("NYK=###"), "#20");
            sut.SendKeys(Ra180Knapp.ÄND);
            Assert.That(sut.Display, Is.EqualTo("NYK=111"), "#21");
            sut.SendKeys(Ra180Knapp.ENT);
            Assert.That(sut.Display, Is.EqualTo("NYK=111"), "#22");
            sut.SendKeys(Ra180Knapp.ENT);
            Assert.That(sut.Display, Is.EqualTo("  (NYK) "), "#23");
            sut.SendKeys(Ra180Knapp.ENT);
            Assert.That(sut.Display, Is.EqualTo(null), "#24");
            sut.SendKeys(Ra180Knapp.Knapp7);
            Assert.That(sut.Display, Is.EqualTo("NYK=111"), "#25");
        }
    }
}
