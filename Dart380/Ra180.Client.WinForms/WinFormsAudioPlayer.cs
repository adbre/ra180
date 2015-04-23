using System;
using System.IO;
using System.Media;
using Ra180.UI;

namespace Ra180.Client.WinForms
{
    internal class WinFormsAudioPlayer : IAudio, IDisposable
    {
        private readonly SoundPlayer _soundPlayer = new SoundPlayer();

        public void Play(AudioFile file)
        {
            OpenAndPlay(file, _soundPlayer.Play);
        }

        public void PlaySync(AudioFile file)
        {
            OpenAndPlay(file, _soundPlayer.PlaySync);
        }

        public void Dispose()
        {
            Stop();
            _soundPlayer.Dispose();
        }

        private void OpenAndPlay(AudioFile file, Action play)
        {
            using (var stream = Open(file))
            {
                if (stream == null)
                    return;

                Stop();
                _soundPlayer.Stream = stream;
                play();
            }
        }

        private Stream Open(AudioFile file)
        {
            switch (file)
            {
                case AudioFile.Data:
                    return Properties.Resources.fmt103_utan_opm;
                case AudioFile.OPM:
                    return Properties.Resources.opm;
                default:
                    return null;
            }
        }

        private void Stop()
        {
            _soundPlayer.Stop();

            var currentStream = _soundPlayer;
            if (currentStream != null)
                currentStream.Dispose();
        }
    }
}