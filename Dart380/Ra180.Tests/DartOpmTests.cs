using Moq;
using NUnit.Framework;
using Ra180.Devices.Dart380;
using Ra180.UI;

namespace Ra180.Tests
{
    [TestFixture]
    public class DartOpmTests
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
            _dart = new Dart380(_synchronizationContext);

            _dart.SendKey(Ra180Key.ModSKYDD);
            _ra180.SendKey(Dart380Key.ModSKYDD);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
        }

        [Test]
        public void OPM_MeddelandeRaderasVidL�sning()
        {
            _dart.SendKey(Dart380Key.ModTE);

            _dart.Mik2 = _ra180;

            _dart.SendKey(Dart380Key.OPM);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("ANSL FTR"));
            _dart.SendKey(Dart380Key.SLT);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("        "));

            _dart.SendKey(Dart380Key.OPM);
            Assert.That(_dart.SmallDisplay.ToString(), Is.Not.EqualTo("ANSL FTR"));
        }

        [Test]
        public void OPM_NyOPMVidOmstart()
        {
            _dart.SendKey(Dart380Key.ModFR�N);

            _dart.Mik2 = _ra180;

            _dart.SendKey(Dart380Key.ModKLAR);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);
            _synchronizationContext.Tick(Ra180.SELFTEST_INTERVAL);

            _dart.SendKey(Dart380Key.OPM);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("ANSL FTR"));
            _dart.SendKey(Dart380Key.OPM);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("ST=FTR/F"));
        }

        [Test]
        [TestCase(Dart380Key.ModTE)]
        public void OPM_Flertr�d_TerminalMod(string mod)
        {
            _dart.SendKey(mod);

            _dart.Mik2 = _ra180;

            _dart.SendKey(Dart380Key.OPM);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("ANSL FTR"));
            _dart.SendKey(Dart380Key.OPM);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("ST=FTR  "));
            _dart.SendKey(Dart380Key.OPM);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo(" (OPM)  "));
            _dart.SendKey(Dart380Key.OPM);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("        "));

            _dart.SendKey(Dart380Key.OPM);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("ST=FTR  "));
            _dart.SendKey(Dart380Key.OPM);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo(" (OPM)  "));
            _dart.SendKey(Dart380Key.OPM);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("        "));
        }

        [Test]
        [TestCase(Dart380Key.ModKLAR)]
        [TestCase(Dart380Key.ModSKYDD)]
        [TestCase(Dart380Key.ModDREL�)]
        public void OPM_Flertr�d_Fj�rrman�vrering(string mod)
        {
            _dart.SendKey(mod);

            _dart.Mik2 = _ra180;

            _dart.SendKey(Dart380Key.OPM);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("ANSL FTR"));
            _dart.SendKey(Dart380Key.OPM);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("ST=FTR/F"));
            _dart.SendKey(Dart380Key.OPM);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo(" (OPM)  "));
            _dart.SendKey(Dart380Key.OPM);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("        "));

            _dart.SendKey(Dart380Key.OPM);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("ST=FTR/F"));
            _dart.SendKey(Dart380Key.OPM);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo(" (OPM)  "));
            _dart.SendKey(Dart380Key.OPM);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("        "));
        }

        [Test]
        [TestCase(Dart380Key.ModTE)]
        public void OPM_Tv�tr�d_TerminalMod(string mod)
        {
            _dart.SendKey(mod);

            _dart.Tv�tr�d = _ra180;

            _dart.SendKey(Dart380Key.OPM);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("ANSL TTR"));
            _dart.SendKey(Dart380Key.OPM);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("ST=TTR  "));
            _dart.SendKey(Dart380Key.OPM);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo(" (OPM)  "));
            _dart.SendKey(Dart380Key.OPM);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("        "));

            _dart.SendKey(Dart380Key.OPM);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("ST=TTR  "));
            _dart.SendKey(Dart380Key.OPM);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo(" (OPM)  "));
            _dart.SendKey(Dart380Key.OPM);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("        "));
        }

        [Test]
        [TestCase(Dart380Key.ModKLAR)]
        [TestCase(Dart380Key.ModSKYDD)]
        [TestCase(Dart380Key.ModDREL�)]
        public void OPM_Tv�tr�d_Fj�rrman�vrering(string mod)
        {
            _dart.SendKey(mod);

            _dart.Tv�tr�d = _ra180;

            _dart.SendKey(Dart380Key.OPM);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("ANSL TTR"));
            _dart.SendKey(Dart380Key.OPM);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("ST=TTR/F"));
            _dart.SendKey(Dart380Key.OPM);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo(" (OPM)  "));
            _dart.SendKey(Dart380Key.OPM);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("        "));

            _dart.SendKey(Dart380Key.OPM);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("ST=TTR/F"));
            _dart.SendKey(Dart380Key.OPM);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo(" (OPM)  "));
            _dart.SendKey(Dart380Key.OPM);
            Assert.That(_dart.SmallDisplay.ToString(), Is.EqualTo("        "));
        }

        [Test]
        public void OPM_Ra180AnslutenP�Tv�tr�d_Operat�rsmeddelandetonSpelasP�Mik1()
        {
            var audio = new Mock<IAudio>();

            _dart.Mik1 = audio.Object;
            _dart.Tv�tr�d = _ra180;

            audio.Verify(m => m.Play(AudioFile.OPM), Times.Once);
        }

        [Test]
        public void OPM_Ra180AnslutenP�Tv�tr�d_Operat�rsmeddelandetonSpelasP�Mik2()
        {
            var audio = new Mock<IAudio>();

            _dart.Mik2 = audio.Object;
            _dart.Tv�tr�d = _ra180;

            audio.Verify(m => m.Play(AudioFile.OPM), Times.Once);
        }

        [Test]
        public void OPM_Ra180AnslutenP�Tv�tr�d_Operat�rsmeddelandetonSpelasP�B�deMik1OchMik2()
        {
            var mik1 = new Mock<IAudio>();
            var mik2 = new Mock<IAudio>();

            _dart.Mik1 = mik1.Object;
            _dart.Mik2 = mik2.Object;
            _dart.Tv�tr�d = _ra180;

            mik1.Verify(m => m.Play(AudioFile.OPM), Times.Once);
            mik2.Verify(m => m.Play(AudioFile.OPM), Times.Once);
        }

        [Test]
        public void OPM_Ra180AnslutenP�Mik2_Operat�rsmeddelandetonSpelasP�Mik1()
        {
            var audio = new Mock<IAudio>();

            _dart.Mik1 = audio.Object;
            _dart.Mik2 = _ra180;

            audio.Verify(m => m.Play(AudioFile.OPM), Times.Once);
        }

        [Test]
        public void OPM_OPMTN_AV_IngetLjudSpelas()
        {
            var mik1 = new Mock<IAudio>();
            var mik2 = new Mock<IAudio>();

            _dart.Mik1 = mik1.Object;
            _dart.Mik2 = mik2.Object;

            _dart.SendKey(Dart380Key.DDA);
            Repeat.NewBuilder()
                .Action(() => _dart.SendKey(Dart380Key.ENT))
                .Until(() => _dart.SmallDisplay.ToString().StartsWith("OPMTN:"))
                .Start();

            Repeat.NewBuilder()
                .Action(() => _dart.SendKey(Dart380Key.�ND))
                .Until(() => _dart.SmallDisplay.ToString().StartsWith("OPMTN:AV"))
                .Start();

            _dart.SendKey(Dart380Key.SLT);

            _dart.Tv�tr�d = _ra180;

            mik1.Verify(m => m.Play(AudioFile.OPM), Times.Never);
            mik2.Verify(m => m.Play(AudioFile.OPM), Times.Never);
        }
    }
}