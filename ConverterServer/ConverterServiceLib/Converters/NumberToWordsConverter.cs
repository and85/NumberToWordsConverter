using System;
using System.Collections.Generic;

namespace AndriiCo.ConverterServer.ConverterServiceLib.Converters
{
    /// <summary>
    /// Class to convert number to words
    /// </summary>
    public class NumberToWordsConverter
    {
        private const string ZeroStr = "zero";
        private const string MillionStr = "million";
        private const string ThousandStr = "thousand";
        private const string HundredStr = "hundred";

        // TODO: introduce digit separators after migration to C# 7.0
        private const int MillionInt = 1000000;
        private const int ThousandInt = 1000;
        private const int HundredInt = 100;
        private const int TenInt = 10;
        private const int NonRecursiveNumber = 21;
        private const int MaxNumber = 999999999;

        Dictionary<int, string> _wordsMap = new Dictionary<int, string>()
        {
            { 0, ZeroStr },
            { 1, "one" },
            { 2, "two" },
            { 3, "three" },
            { 4, "four" },
            { 5, "five" },
            { 6, "six" },
            { 7, "seven" },
            { 8, "eight" },
            { 9, "nine" },
            { 10, "ten" },
            { 11, "eleven" },
            { 12, "twelve" },
            { 13, "thirteen" },
            { 14, "fourteen" },
            { 15, "fifteen" },
            { 16, "sixteen" },
            { 17, "seventeen" },
            { 18, "eighteen" },
            { 19, "nineteen" },
            { 20, "twenty" },
            { 30, "thirty" },
            { 40, "forty" },
            { 50, "fifty" },
            { 60, "sixty" },
            { 70, "seventy" },
            { 80, "eighty" },
            { 90, "ninety" },
        };

        /// <summary>
        /// Converts number to words
        /// </summary>
        /// <param name="number">Input number</param>
        /// <exception cref="ArgumentException">Throws ArgumentException when the input number can't be converted to words</exception>
        /// <returns>String of words</returns>
        public string Convert(int number)
        {
            if (number > MaxNumber || number < 0)
                throw new ArgumentException($"Number {number} should be in a range between 0 and {MaxNumber}!");

            string result;
            string endingZero = $" {ZeroStr}";

            // recursively devide input number until we reach the biggest number that could be get without recursion 
            if (number >= MillionInt)
            {
                result = Convert(number / MillionInt) + $" {MillionStr} "+ Convert(number % MillionInt);
                return result.Replace(endingZero, string.Empty);
            }
            if (number >= ThousandInt)
            {
                result = Convert(number / ThousandInt) + $" {ThousandStr} " + Convert(number % ThousandInt);
                return result.Replace(endingZero, string.Empty);
            }
            if (number >= HundredInt)
            {
                result = Convert(number / HundredInt) + $" {HundredStr} " + Convert(number % HundredInt);
                return result.Replace(endingZero, string.Empty);
            }
            if (number >= NonRecursiveNumber)
            {
                return _wordsMap[number / TenInt * TenInt] + "-" + _wordsMap[number % TenInt];
            }
            
            return _wordsMap[number];
        }
    }
}
