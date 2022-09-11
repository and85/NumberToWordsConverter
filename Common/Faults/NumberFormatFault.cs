using System.Runtime.Serialization;

// ReSharper disable once CheckNamespace
// Namespaces between ServiceContract and Service should match
namespace AndriiCo.ConverterServer.ConverterServiceLib
{
    /// <summary>
    /// Exception that happens when the input string can't be converted
    /// </summary>
    [DataContract]
    public class NumberFormatFault
    {
        private string _report;

        /// <summary>
        /// Construction of the fault
        /// </summary>
        /// <param name="message">Text that will be thrown to the client</param>
        public NumberFormatFault(string message)
        {
            _report = message;
        }

        /// <summary>
        /// Exception text
        /// </summary>
        [DataMember]
        public string Message
        {
            get { return _report; }
            set { _report = value; }
        }
    }
}
