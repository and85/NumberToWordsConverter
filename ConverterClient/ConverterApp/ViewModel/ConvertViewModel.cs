using System;
using System.Reflection;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using log4net;
using AndriiCo.ConverterClient.ConverterApp.DataAccess;
using AndriiCo.ConverterClient.ConverterApp.Model;

namespace AndriiCo.ConverterClient.ConverterApp.ViewModel
{
    /// <summary>
    /// ViewModel to convert money from number to words
    /// </summary>
    public class ConvertViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IConverterServiceClient _serviceClient;

        private readonly Money _money;

        private bool _isProgressBarVisible;
        private bool _canRun = true;
        private bool _canClear;

        /// <summary>
        /// Amount of money 
        /// </summary>
        public string AmountNumber
        {
            get { return _money.AmountNumber; }
            set
            {
                _money.AmountNumber = value;
                RaisePropertyChanged("AmountNumber");
            }
        }

        /// <summary>
        /// Amount of money in words
        /// </summary>
        public string AmountWords
        {
            get { return _money.AmountWords; }
            set
            {
                _money.AmountWords = value;
                RaisePropertyChanged("AmountWords");
            }
        }

        /// <summary>
        /// Flag that shows if <see cref="AmountNumber"/> were converted to <see cref="AmountWords"/> successfully
        /// </summary>
        public bool IsError
        {
            get { return _money.IsError; }
            set
            {
                _money.IsError = value;
                RaisePropertyChanged("IsError");
            }
        }

        /// <summary>
        /// Shows if <see cref="ConvertCommand"/> command can execute
        /// </summary>
        public bool CanRun
        {
            get { return _canRun; }
            set
            {
                _canRun = value;
                RaisePropertyChanged("CanRun");
            }
        }

        /// <summary>
        /// Shows if <see cref="ClearCommand"/> command can execute
        /// </summary>
        public bool CanClear
        {
            get { return _canClear; }
            set
            {
                _canClear = value;
                RaisePropertyChanged("CanClear");
            }
        }

        /// <summary>
        /// Shows if the progress bar should be visible 
        /// </summary>
        public bool IsProgressBarVisible
        {
            get { return _isProgressBarVisible; }
            set
            {
                _isProgressBarVisible = value;
                RaisePropertyChanged("IsProgressBarVisible");
            }
        }

        /// <summary>
        /// Command to clear result of a previous convertation
        /// </summary>
        public RelayCommand ClearCommand { get; private set; }

        /// <summary>
        ///  Command to convert money from number to words 
        /// </summary>
        public RelayCommand ConvertCommand { get; }

        /// <summary>
        /// Initialises a new instance of ConvertViewModel class
        /// </summary>
        /// <param name="serviceClient">WCF client</param>
        public ConvertViewModel(IConverterServiceClient serviceClient)
        {
            _serviceClient = serviceClient;

            _money = new Money();
            IsProgressBarVisible = false;

            ClearCommand = new RelayCommand(ClearExecute, () => CanClear);
            ConvertCommand = new RelayCommand(ConvertExecuteAsync, () => CanRun);
        }

        internal void ClearExecute()
        {
            Log.Info("Starting ClearExecute...");

            AmountWords = string.Empty;
            IsError = false;
            CanClear = false;

            Log.Info("ClearExecute finished.");
        }

        internal async void ConvertExecuteAsync()
        {
            Log.Info($"Starting ConvertExecuteAsync operation for {AmountNumber}...");

            var convertedNumber = string.Empty;
            try
            {
                SetCommandsCanRun(false);                
                convertedNumber = await _serviceClient.ConvertAsync(AmountNumber);
            }
            catch (Exception ex)
            {
                IsError = true;
                convertedNumber = $"In the real world due to security reasons we way want to hide a real exception from the user!" +
                                  $"\n{ex.Message}";

            }
            finally
            {
                SetCommandsCanRun(true);
                CanClear = true;
            }

            AmountWords = convertedNumber;

            Log.Info("ConvertExecuteAsync finished");
        }

        private void SetCommandsCanRun(bool canRun)
        {
            CanRun = canRun;            
            IsProgressBarVisible = !canRun;
            ConvertCommand.RaiseCanExecuteChanged();
        }
    }
}
