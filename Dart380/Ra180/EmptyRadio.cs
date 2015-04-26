using System;

namespace Ra180
{
    public class EmptyRadio : IRadio
    {
        public bool IsStarted { get; private set; }

        public event EventHandler ReceivedSynk;
        public event MessageEventHandler Received;

        public void Start()
        {
            OnStart();
        }

        public void Stop()
        {
            OnStop();
        }

        protected virtual void OnStart()
        {
            IsStarted = true;
        }

        protected virtual void OnStop()
        {
            IsStarted = false;
        }

        public virtual void SetChannelData(Ra180ChannelData channelData, DateTime dateTime)
        {
        }

        public virtual bool SendDataMessage(MessageEventArgs message, Action callback)
        {
            return false;
        }

        protected virtual void OnReceivedSynk()
        {
            EventHandler handler = ReceivedSynk;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        protected virtual void OnReceived(MessageEventArgs args)
        {
            MessageEventHandler handler = Received;
            if (handler != null) handler(this, args);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                
            }
        }
    }
}