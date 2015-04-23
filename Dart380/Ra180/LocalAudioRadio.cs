using System;
using System.Threading.Tasks;
using Ra180.UI;

namespace Ra180
{
    public class LocalAudioRadio : IRadio, IDisposable
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
            var networkComplete = false;
            Task.Run(() =>
            {
                _audio.PlaySync(AudioFile.Data);

                while (!networkComplete)
                {
                    Task.Delay(200);
                }

                callback();
            });
            return _radio.SendDataMessage(message, () => networkComplete = true);
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
            _audio.PlaySync(AudioFile.Data);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            throw new NotImplementedException();
        }
    }
}