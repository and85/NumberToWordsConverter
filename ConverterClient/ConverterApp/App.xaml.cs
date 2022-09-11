using System;
using System.Reflection;
using System.Windows;
using log4net;
using log4net.Config;
using log4net.Repository.Hierarchy;

namespace AndriiCo.ConverterClient.ConverterApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public App()
        {
            // initialise log4net
            XmlConfigurator.Configure();

            var currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = (Exception)e.ExceptionObject;
            Log.Error($"An unhandled exception has happened. Exception: {ex}", ex);
        }
    }
}
