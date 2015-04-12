using System;

namespace Ra180
{
    public class EmptyRadio : IRadio
    {
        public event EventHandler ReceivedSynk;
        public event MessageEventHandler Received;

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
    }
}