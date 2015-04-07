using System;

namespace Ra180
{
    public class EmptyRadio : IRadio
    {
        public event EventHandler ReceivedSynk;

        public virtual void SetChannelData(Ra180ChannelData channelData, DateTime dateTime)
        {
        }

        public virtual bool SendDataMessage(string[] data, Action callback)
        {
            return false;
        }

        protected virtual void OnReceivedSynk()
        {
            EventHandler handler = ReceivedSynk;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }
}