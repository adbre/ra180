using System;
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

        [Test]
        public void Should_be_59_seconds_per_minute()
        {
            _ra180.SendKey(Ra180KeyCode.TID);
            _synchronizationContext.Tick(TimeSpan.FromSeconds(59));
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:000059"));
            _synchronizationContext.Tick(TimeSpan.FromSeconds(1));
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:000100"));
        }

        [Test]
        public void Should_be_59_minutes_per_hour()
        {
            _ra180.SendKey(Ra180KeyCode.TID);
            _synchronizationContext.Tick(TimeSpan.FromMinutes(59));
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:005900"));
            _synchronizationContext.Tick(TimeSpan.FromMinutes(1));
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:010000"));
        }

        [Test]
        public void Should_be_24_hours_per_day()
        {
            _ra180.SendKey(Ra180KeyCode.TID);
            _synchronizationContext.Tick(TimeSpan.FromHours(23));
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:230000"));
            _synchronizationContext.Tick(TimeSpan.FromHours(1));
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("T:000000"));
            _ra180.SendKey(Ra180KeyCode.ENT);
            Assert.That(_ra180.Display.ToString(), Is.EqualTo("DAT:0102"));
        }
    }
}