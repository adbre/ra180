using System.Threading;
using NUnit.Framework;

namespace Ra180.Tests
{
    [TestFixture]
    public class InstantSynchronizationContextTests
    {
        private InstantSynchronizationContext GetSystemUnderTest()
        {
            return new InstantSynchronizationContext();
        }

        [Test]
        public void Repeat_CallbackIsNeverCalled()
        {
            var callbackWasNeverCalled = true;
            GetSystemUnderTest().Repeat(() => callbackWasNeverCalled = false, 10);

            Thread.Sleep(500);

            Assert.That(callbackWasNeverCalled);
        }

        [Test]
        public void Schedule_CallbackWasInvokedImmediatly()
        {
            var callbackWasCalled = false;
            GetSystemUnderTest().Schedule(() => callbackWasCalled = true, 500);

            Assert.That(callbackWasCalled);
        }

        [Test]
        public void Cancel_RepeatToken_DoesNotThrow()
        {
            var sut = GetSystemUnderTest();
            var token = sut.Repeat(() => { }, 10);

            Assert.DoesNotThrow(() => sut.Cancel(token));
        }

        [Test]
        public void Cancel_ScheduleToken_DoesNotThrow()
        {
            var sut = GetSystemUnderTest();
            var token = sut.Schedule(() => { }, 10);

            Assert.DoesNotThrow(() => sut.Cancel(token));
        }
    }
}
