using System;
using System.Configuration;
using System.Reflection;
using System.ServiceModel;
using System.Threading.Tasks;
using log4net;
using AndriiCo.ConverterServer.ConverterServiceLib;

namespace AndriiCo.ConverterClient.ConverterApp.DataAccess
{
    /// <summary>
    /// WCF service client
    /// </summary>
    public class ConverterServiceClient : IConverterServiceClient
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Asynchronous convert operation
        /// </summary>
        /// <param name="input">String that should be converted from number to representation of money in words</param>        
        /// <returns>Task that results in representation of money in words</returns>
        public async Task<string> ConvertAsync(string input)
        {
            string endpointName = ConfigurationManager.AppSettings["CurrentWcfEndpoint"];

            using (var clientWrapper = new ServiceClientWrapper<IConverterService>(endpointName))
            {
                try
                {
                    return await clientWrapper.Channel.ConvertAsync(input);
                }
                catch (TimeoutException timeoutException)
                {
                    Log.Error($"TimeoutException has happened. Exception: {timeoutException}");
                    throw;
                }
                catch (FaultException<NumberFormatFault> numberFormatFault)
                {
                    throw new ArgumentException($"Coulnd't parse input value '{input}'. Exception: {numberFormatFault.Detail.Message}");
                }
                catch (FaultException unknownFault)
                {
                    Log.Error($"UnknownFault has happened. Exception: {unknownFault}");
                    throw;
                }
                catch (CommunicationException communicationException)
                {
                    Log.Error($"CommunicationException has happened. Exception: {communicationException}");
                    throw;
                }
            }
        }
    }
}
