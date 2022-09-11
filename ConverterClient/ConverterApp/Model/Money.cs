namespace AndriiCo.ConverterClient.ConverterApp.Model
{
    /// <summary>
    /// Data model used to store number and in words represenation of money
    /// </summary>
    public class Money
    {
        /// <summary>
        /// Amount of money 
        /// </summary>
        public string AmountNumber { get; set; }

        /// <summary>
        /// Amount of money in words
        /// </summary>
        public string AmountWords { get; set; }

        /// <summary>
        /// Flag that shows if <see cref="AmountNumber"/> were converted to <see cref="AmountWords"/> successfully
        /// </summary>
        public bool IsError { get; set; }
    }
}
