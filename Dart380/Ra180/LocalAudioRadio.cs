using System;
using Ra180.UI;

namespace Ra180
{
    public class LocalAudioRadio : IRadio
    {
        private readonly IRadio _radio;
        private readonly IAudio _audio;

        public LocalAudioRadio(IRadio radio, IAudio audio)
        {
            if (radio == null) throw new ArgumentNullException("radio");
            if (audio == null) throw new ArgumentNullException("audio");
            _radio = radio;
            _audio = audio;

            _radio.Received += RadioOnReceived;
        }

        public void SetChannelData(Ra180ChannelData channelData, DateTime dateTime)
        {
            _radio.SetChannelData(channelData, dateTime);
        }

        public event MessageEventHandler Received;

        public bool SendDataMessage(MessageEventArgs message, Action callback)
        {
            PlayDataAudio();
            return _radio.SendDataMessage(message, callback);
        }

        protected virtual void OnReceived(MessageEventArgs args)
        {
            var handler = Received;
            if (handler != null) handler(this, args);
        }

        private void RadioOnReceived(object sender, MessageEventArgs args)
        {
            PlayDataAudio();
            OnReceived(args);
        }

        private void PlayDataAudio()
        {
            _audio.Play(AudioFile.Data);
        }
    }
}