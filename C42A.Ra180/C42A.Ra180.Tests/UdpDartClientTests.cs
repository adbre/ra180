using System;
using System.Threading;
using C42A.Ra180.Infrastructure.UdpDart;
using NUnit.Framework;

namespace C42A.Ra180.Tests
{
    [TestFixture]
    public class UdpDartClientTests
    {
        [Test]
        public void ShouldSendAndReceive()
        {
            const string expected = "Fredrik är gammal, Cecilia är ung.";

            using (var client1 = new UdpClientSenderAndReceiver())
            using (var client2 = new UdpClientSenderAndReceiver())
            using (var client3 = new UdpClientSenderAndReceiver())
            {
                var actual1 = default(string);
                var actual2 = default(string);
                client2.ReceivedString += (sender, s) => actual1 = s;
                client3.ReceivedString += (sender, s) => actual2 = s;
                client2.StartAsync();
                client3.StartAsync();
                client1.SendString(expected);

                Thread.Sleep(TimeSpan.FromSeconds(2));

                Assert.That(actual1, Is.EqualTo(expected));
                Assert.That(actual2, Is.EqualTo(expected));
            }
        }
    }
}