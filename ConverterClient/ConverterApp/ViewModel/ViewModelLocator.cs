/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:ConverterApp"
                           x:Key="Locator" />
  </Application.Resources>

  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using System;
using GalaSoft.MvvmLight.Ioc;
using log4net.Config;
using Microsoft.Practices.ServiceLocation;
using AndriiCo.ConverterClient.ConverterApp.DataAccess;

namespace AndriiCo.ConverterClient.ConverterApp.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            // initialise log4net
            XmlConfigurator.Configure();

            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<IConverterServiceClient, ConverterServiceClient>();
            var convertViewModel = new ConvertViewModel(SimpleIoc.Default.GetInstance<IConverterServiceClient>());
            var mainViewModel = new MainViewModel(convertViewModel);
            SimpleIoc.Default.Register(() => mainViewModel);
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
    }
}

