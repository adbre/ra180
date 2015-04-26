using Moq;
using NUnit.Framework;

namespace Ra180.Tests
{
    [TestFixture]
    public class Ra180RadioTests
    {
        private Ra180 _ra180;
        private DelayedSynchronizationContext _synchronizationContext;
        private EmptyRadio _radio;

        [SetUp]
        public void SetUp()
        {
            _radio = new EmptyRadio();
            _synchronizationContext = new DelayedSynchronizationContext();
            _ra180 = new Ra180(_radio, _synchronizationContext);
        }

        [Test]
        public void StartsRadio()
        {
            _ra180.SendKey(Ra180Key.ModFRÅN);
            Assert.That(_radio.IsStarted, Is.False, "#1");

            _ra180.SendKey(Ra180Key.ModKLAR);
            _synchronizationContext.Tick(Ra180.SELFTEST);
            Assert.That(_radio.IsStarted, Is.True, "#2");

            _ra180.SendKey(Ra180Key.ModSKYDD);
            Assert.That(_radio.IsStarted, Is.True, "#3");

            _ra180.SendKey(Ra180Key.ModDRELÄ);
            Assert.That(_radio.IsStarted, Is.True, "#4");

            _ra180.SendKey(Ra180Key.ModFRÅN);
            Assert.That(_radio.IsStarted, Is.False, "#5");
        }

        [Test]
        public void DisposesOnShutdown()
        {
            var radio = new Mock<IRadio>();
            var ra180 = new Ra180(radio.Object, _synchronizationContext);

            ra180.SendKey(Ra180Key.ModKLAR);
            _synchronizationContext.Tick(Ra180.SELFTEST);
            ra180.SendKey(Ra180Key.ModFRÅN);

            radio.Verify(m => m.Dispose(), Times.Once);
        }

        [Test]
        public void DisplayErrorFromRadio()
        {
            var radio = new Mock<IRadio>();
            var ra180 = new Ra180(radio.Object, _synchronizationContext);

            radio.Setup(m => m.Start()).Throws(new Ra180Exception("#NETWORK"));

            ra180.SendKey(Ra180Key.ModKLAR);
            _synchronizationContext.Tick(Ra180.SELFTEST);
            Assert.That(ra180.Display.ToString(), Is.EqualTo("#NETWORK"));
        }
    }
}