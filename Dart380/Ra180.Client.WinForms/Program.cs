using System;
using System.IO;
using System.Media;
using System.Windows.Forms;
using Ra180.Devices.Dart380;
using Ra180.Transports;
using Ra180.UI;

namespace Ra180.Client.WinForms
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var synchronizationContext = new TimerSynchronizationContext())
            using (var signalR = new SignalRTransport(Properties.Settings.Default.Server))
            using (var soundPlayer = new WinFormsAudioPlayer())
            {
                synchronizationContext.Start();
                signalR.StartAsync();

                var transport = new LocalAudioRadio(signalR, soundPlayer);
                var ra180 = new Ra180(transport, synchronizationContext);
                ra180.SendKey(Ra180Key.ModKLAR);

                var dart380 = new Dart380(synchronizationContext);
                dart380.Mik2 = ra180;
                dart380.Mik1 = soundPlayer;

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Dart380Form { Dart380 = dart380 });
            }
        }
    }

    internal class WinFormsAudioPlayer : IAudio, IDisposable
    {
        private readonly SoundPlayer _soundPlayer = new SoundPlayer();

        public void Play(AudioFile file)
        {
            switch (file)
            {
                case AudioFile.Data:
                    Play(Properties.Resources.fmt103);
                    break;

                case AudioFile.OPM:
                    Play(Properties.Resources.opm);
                    break;
            }
        }

        public void Play(Stream stream)
        {
            _soundPlayer.Stop();

            var currentStream = _soundPlayer;
            if (currentStream != null)
                currentStream.Dispose();

            _soundPlayer.Stream = stream;
            _soundPlayer.Play();
        }

        public void Dispose()
        {
            _soundPlayer.Dispose();
        }
    }
}
