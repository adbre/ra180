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
            using (var radio = new SignalRTransport(Properties.Settings.Default.Server))
            {
                synchronizationContext.Start();
                radio.StartAsync();

                var ra180 = new Ra180(radio, synchronizationContext);
                ra180.SendKey(Ra180Key.ModKLAR);

                var dart380 = new Dart380(synchronizationContext);
                dart380.Ra180 = ra180;

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Dart380Form { Dart380 = dart380 });
            }
        }
    }
}
