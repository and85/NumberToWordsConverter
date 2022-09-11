using System;
using System.Reflection;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using AndriiCo.ConverterServer.ConverterServiceLib.Converters;

namespace AndriiCo.ConverterServer.ConverterServiceLib
{
    /// <summary>
    /// WCF service than implements <see cref="IConverterService"/>
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class ConverterService: IConverterService
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const char Separator = ',';
        private const string Space = " ";
        private const string MajorCurrencyPart = "dollar";
        private const string MinorCurrencyPart = "cent";

        /// <summary>
        /// Regular expression to validate initialInput data
        /// ^ - means the beginning of a string
        /// ([0-9]){1,9} - numbers should appear from 1 to 9 times 
        /// ([,][0-9][0-9])? - an optional part that contains one comma separator and exactly 2 numbers
        /// $ - means the end of a string
        /// </summary>
        private string RegexPattern = @"^([0-9]){1,9}([,][0-9][0-9])?";

        /// <summary>
        /// Synchronous convert operation
        /// </summary>
        /// <param name="initialInput">String that should be converted from number to representation of money in words</param>
        /// <exception cref="NumberFormatFault">Exception that happens when the input string can't be converted</exception>
        /// <returns>Representation of money in words</returns>
        public string Convert(string initialInput)
        {
            Log.Info($"Start converting the string '{initialInput}'...");
#if !DEBUG
            // just for the sake of Async demonstration pretend that we have some heavy calculations here 
            Thread.Sleep(2000);
#endif
            var inputWithoutSpaces = ValidateInput(initialInput);
            var parts = inputWithoutSpaces.Split(Separator);
            var result = GetFinalString(parts);
            Log.Info($"Convert finished.");
            return result;
        }

        /// <summary>
        /// Asynchronous convert operation
        /// </summary>
        /// <param name="initialInput">String that should be converted from number to representation of money in words</param>
        /// <exception cref="NumberFormatFault">Exception that happens when the input string can't be converted</exception>
        /// <returns>Returns Task that results in representation of money in words</returns>
        public async Task<string> ConvertAsync(string initialInput)
        {
            Log.Info($"Start converting input string '{initialInput}' asynchronously...");

            var task = Task.Factory.StartNew(() => Convert(initialInput));
            string result = string.Empty;
            try
            {
                result = await task;
            }
            catch (AggregateException ae)
            {
                Log.Error($"AggregateException has happened! Exception: {ae}");

                ae.Handle(x =>
                {
                    if (x is FaultException<NumberFormatFault>)
                    {
                        Log.Error($"An error has happened! Exception: {x}");
                        throw x;
                    }
                    return false; 
                });
            }

            Log.Info("Asynchronous convert finished.");
            return result;
        }

        private string ValidateInput(string initialInput)
        {
            if (string.IsNullOrEmpty(initialInput))
            {
                var error = "Input string can't be empty!";
                Log.Error(error);
                throw new FaultException<NumberFormatFault>(
                    new NumberFormatFault(error));
            }

            // remove spaces from the initial string, that simlifies regex and further parsing logic
            var inputWithoutSpaces = initialInput.Replace(Space, string.Empty);
            if (Regex.Match(inputWithoutSpaces, RegexPattern).Length != inputWithoutSpaces.Length)
            {
                var error = $"Input string '{initialInput}' should match a regex pattern '{RegexPattern}'!";
                Log.Error(error);
                throw new FaultException<NumberFormatFault>(new NumberFormatFault(error));
            }
            return inputWithoutSpaces;
        }

        private string GetFinalString(string[] parts)
        {
            var converter = new NumberToWordsConverter();

            var dollarPart = int.Parse(parts[0]);
            var dollars = converter.Convert(dollarPart)
                          + $"{Space}{MajorCurrencyPart}"
                          + GetCurrencyEnding(dollarPart);

            string cents = string.Empty;
            if (parts.Length > 1)
            {
                var centPart = int.Parse(parts[1]);
                cents = $"{Space}and "
                        + converter.Convert(centPart)
                        + $"{Space}{MinorCurrencyPart}"
                        + GetCurrencyEnding(centPart);
            }

            var result = dollars + cents;
            return result;
        }

        private string GetCurrencyEnding(int number)
        {
            if (number % 10 != 1)
                return "s";

            return string.Empty;
        }
    }
}
 