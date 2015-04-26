using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

namespace Ra180.Transports
{
    public class SignalRTransport : IRadio, IDisposable
    {
        private IHubProxy _hubProxy;
        private readonly HubConnection _connection;

        public SignalRTransport(string url)
            : this(new HubConnection(url))
        {
        }

        public SignalRTransport(HubConnection connection)
        {
            _connection = connection;
            _hubProxy = connection.CreateHubProxy("Ra180Hub");
            _hubProxy.On("Receive", (string message) => OnReceived(new MessageEventArgs(message)));
        }

        public event MessageEventHandler Received;

        public async void StartAsync()
        {
            await _connection.Start();
        }

        public bool SendDataMessage(MessageEventArgs message, Action callback)
        {
            Task.Run(() => SendDataMessageAsync(message, callback));
            return true;
        }

        public void Start()
        {
            _connection.Start();
        }

        public void Stop()
        {
            _connection.Stop();
        }

        public void SetChannelData(Ra180ChannelData channelData, DateTime dateTime)
        {
            
        }

        public void Dispose()
        {
            OnDispose(true);
            GC.SuppressFinalize(this);
        }

        private void OnDispose(bool disposing)
        {
            if (disposing)
            {
                _connection.Stop();
                _connection.Dispose();
            }
        }

        protected virtual void OnReceived(MessageEventArgs args)
        {
            var handler = Received;
            if (handler != null) handler(this, args);
        }

        private async void SendDataMessageAsync(MessageEventArgs message, Action callback)
        {
            await _hubProxy.Invoke("Send", message.Message);
            callback();
        }
    }
}
