using System.Threading.Tasks;

namespace AndriiCo.ConverterClient.ConverterApp.DataAccess
{
    /// <summary>
    /// Interface for WCF service client
    /// </summary>
    public interface IConverterServiceClient
    {
        /// <summary>
        /// Asynchronous convert operation
        /// </summary>
        /// <param name="input">String that should be converted from number to representation of money in words</param>        
        /// <returns>Task that results in representation of money in words</returns>
        Task<string> ConvertAsync(string input);
    }
}
