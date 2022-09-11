using System;
using System.ServiceModel;
using AndriiCo.ConverterServer.ConverterServiceLib;
using System.Reflection;
using log4net;
using log4net.Config;

namespace AndriiCo.ConverterServer.ConverterHostApp
{
    /// <summary>
    /// Console application to host WCF service
    /// </summary>
    class Program
    {
        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            // initialise log4net
            XmlConfigurator.Configure();

            Log.Info("Starting the service...");

            ServiceHost svcHost = null;
            try
            {
                svcHost = new ServiceHost(typeof(ConverterService));
                svcHost.Open();
                Log.Info("Service is running...");
            }
            catch (Exception ex)
            {
                Log.Fatal($"Service couln't start. Error message: {ex.Message}");
                return;
            }

            Log.Info("Press Enter to close the service");
            Console.ReadLine();
            svcHost.Close();
        }
    }
}
