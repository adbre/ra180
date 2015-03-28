using System.Collections.Generic;
using NUnit.Framework;

namespace Ra180.Tests
{
    [TestFixture]
    public class DelayedSynchronizationContextTests
    {
        private DelayedSynchronizationContext GetSystemUnderTest()
        {
            return new DelayedSynchronizationContext();
        }

        [Test]
        [TestCase(0, ExpectedResult = 0)]
        [TestCase(499, ExpectedResult = 0)]
        [TestCase(500, ExpectedResult = 1)]
        [TestCase(999, ExpectedResult = 1)]
        [TestCase(1000, ExpectedResult = 2)]
        public int Repeat(int tick)
        {
            var callbackWasCalled = 0;

            var sut = GetSystemUnderTest();
            sut.Repeat(() => callbackWasCalled++, 500);

            sut.Tick(tick);

            return callbackWasCalled;
        }

        [Test]
        [TestCase(0, ExpectedResult = 0)]
        [TestCase(499, ExpectedResult = 0)]
        [TestCase(500, ExpectedResult = 1)]
        [TestCase(999, ExpectedResult = 1)]
        [TestCase(1000, ExpectedResult = 1)]
        public int Schedule(int tick)
        {
            var callbackWasCalled = 0;

            var sut = GetSystemUnderTest();
            sut.Schedule(() => callbackWasCalled++, 500);

            sut.Tick(tick);

            return callbackWasCalled;
        }

        [Test]
        public void Flow()
        {
            var callOrder = new List<int>();

            var sut = GetSystemUnderTest();
            sut.Schedule(() => callOrder.Add(1), 40);
            sut.Repeat(() => callOrder.Add(2), 10);
            sut.Schedule(() => callOrder.Add(3), 100);
            sut.Schedule(() => callOrder.Add(4), 70);

            sut.Tick(100);

            Assert.That(callOrder, Is.EqualTo(new []
            {
                2,2,2,
                1,2,2,
                2,4,2,
                2,2,3,
                2
            }));
        }

        [Test]
        public void Cancel()
        {
            var callbackWasNeverCalled = true;

            var sut = GetSystemUnderTest();
            var token = sut.Schedule(() => callbackWasNeverCalled = false, 100);
            sut.Tick(50);
            sut.Cancel(token);
            sut.Tick(50);

            Assert.That(callbackWasNeverCalled);
        }
    }
}