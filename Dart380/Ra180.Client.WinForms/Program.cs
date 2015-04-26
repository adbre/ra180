using System;
using System.Windows.Forms;
using Ra180.Devices.Dart380;
using Ra180.Transports;

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

                var signalRFactory = new SimpleRadioFactory(() =>
                {
                    var settings = Properties.Settings.Default;
                    settings.Reload();
                    return new SignalRTransport(settings.Server);
                });

                var transport = new LocalAudioRadio(signalR, soundPlayer);
                var ra180 = new Ra180(signalRFactory, synchronizationContext);
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
}
