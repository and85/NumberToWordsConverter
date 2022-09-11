using System.ServiceModel;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
// Namespaces between ServiceContract and Service should match
namespace AndriiCo.ConverterServer.ConverterServiceLib
{
    /// <summary>
    /// Interface that defines communication between WCF service and client
    /// </summary>
    [ServiceContract]
    public interface IConverterService
    {
        /// <summary>
        /// Synchronous convert operation
        /// </summary>
        /// <param name="input">String that should be converted from number to representation of money in words</param>
        /// <exception cref="NumberFormatFault">Exception that happens when the input string can't be converted</exception>
        /// <returns>Returns representation of money in words</returns>
        [OperationContract(Name = "ConvertSync")]
        [FaultContract(typeof(NumberFormatFault))]
        string Convert(string input);

        /// <summary>
        /// Asynchronous convert operation
        /// </summary>
        /// <param name="input">String that should be converted from number to representation of money in words</param>
        /// <exception cref="NumberFormatFault">Exception that happens when the input string can't be converted</exception>
        /// <returns>Returns Task that results in representation of money in words</returns>
        [OperationContract]
        [FaultContract(typeof(NumberFormatFault))]
        Task<string> ConvertAsync(string input);
    }
}
