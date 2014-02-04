using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace C42A.Ra180.Infrastructure.UdpDart
{
    public class UdpClientSenderAndReceiver : IDisposable, IDartTransport
    {
        private const int Port = 3032;
        private readonly UdpClient _receiver;
        private readonly UdpClient _sender;
        private bool _listening;
        private IPEndPoint _receiverEp = new IPEndPoint(IPAddress.Any, Port);

        public UdpClientSenderAndReceiver()
        {
            _receiver = new UdpClient();
            _receiver.Client.SetSocketOption(
                SocketOptionLevel.Socket,
                SocketOptionName.ReuseAddress,
                true
            );
            _receiver.ExclusiveAddressUse = false;
            _receiver.Client.Bind(_receiverEp);
            _sender = new UdpClient();
        }

        public event EventHandler<string> ReceivedString;

        protected virtual void OnReceivedString(string e)
        {
            var handler = ReceivedString;
            if (handler != null) handler(this, e);
        }

        public void StartAsync()
        {
            if (_listening) return;
            var thread = new Thread(ReceiveAsync);
            _listening = true;
            thread.IsBackground = true;
            thread.Start();
        }

        private void ReceiveAsync()
        {
            while (_listening)
            {
                TryReceiveOne();
                Thread.Sleep(150);
            }
        }

        private void TryReceiveOne()
        {
            //if (_udpClient.Available < 1) return;
            var bytes = _receiver.Receive(ref _receiverEp);
            OnReceivedBytes(bytes);
        }

        private void OnReceivedBytes(byte[] bytes)
        {
            var text = Encoding.UTF8.GetString(bytes);
            OnReceivedString(text);
        }

        public void Dispose()
        {
            _listening = false;
            _receiver.Close();
            _sender.Close();
        }

        public void SendString(string s)
        {
            var bytes = Encoding.UTF8.GetBytes(s);
            SendBytes(bytes);
        }

        private void SendBytes(byte[] bytes)
        {
            var broadcast = new IPEndPoint(IPAddress.Broadcast, Port);
            _sender.Send(bytes, bytes.Length, broadcast);
        }
    }
}
