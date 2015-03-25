using NUnit.Framework;

namespace Ra180.Tests
{
    [TestFixture]
    public class Ra180DisplayTests
    {
        [Test]
        public void SetText_RaisesChangedEventOnce()
        {
            var callCount = 0;

            var sut = new Ra180Display();
            sut.Changed += (sender, args) => callCount++;

            sut.SetText("TEST");

            Assert.That(callCount, Is.EqualTo(1));
        }
    }
}
