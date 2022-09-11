using GalaSoft.MvvmLight;

namespace AndriiCo.ConverterClient.ConverterApp.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// The current view.
        /// </summary>
        private ViewModelBase _currentViewModel;

        

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(ViewModelBase initialViewModel)
        {
            CurrentViewModel = initialViewModel;
        }

        /// <summary>
        /// The CurrentView property.  
        /// </summary>
        public ViewModelBase CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set
            {
                if (_currentViewModel == value)
                    return;

                _currentViewModel = value;
                RaisePropertyChanged("CurrentViewModel");
            }
        }
    }
}