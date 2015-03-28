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
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("NYK=### "));
            _ra180.SendKey(Ra180Key.ÄND);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("NYK=### "));
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
            Helper.EnterNewPny(_ra180, key);
        }
    }
}