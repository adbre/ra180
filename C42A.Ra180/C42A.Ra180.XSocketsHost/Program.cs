using System.ServiceProcess;

namespace C42A.Ra180.XSocketsHost
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new XSocketServerHost() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
