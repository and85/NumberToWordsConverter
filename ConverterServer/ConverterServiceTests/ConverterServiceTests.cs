using System.Collections;
using System.ServiceModel;
using NUnit.Framework;
using AndriiCo.ConverterServer.ConverterServiceLib;

namespace AndriiCo.ConverterServer.ConverterServiceTests
{
    /// <summary>
    /// Tests for <see cref="ConverterService"/>
    /// </summary>
    [TestFixture]
    public class ConverterServiceTests
    {
        private ConverterService _converterService;

        [OneTimeSetUp]
        public void Init()
        {
            _converterService = new ConverterService();
        }

        [Test, TestCaseSource(typeof(ConverterServiceTests), nameof(ShouldReturnCorrectResultTestCases))]
        public void Convert_ReturnsCorrectResult(string input, string expected)
        {
            // Act 
            var actual = _converterService.Convert(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test, TestCaseSource(typeof(ConverterServiceTests), nameof(ShouldThrowFaultExceptionTestCases))]
        public void Convert_ThrowsFaultException_WhenInputHasWrongFormat(string input)
        {
            // Act and Assert
            Assert.Throws<FaultException<NumberFormatFault>>(() => _converterService.Convert(input));
        }

        [Test, TestCaseSource(typeof(ConverterServiceTests), nameof(ShouldReturnCorrectResultTestCases))]
        public void ConvertAsync_ReturnsCorrectResult(string input, string expected)
        {
            // Act 
            var actual = _converterService.ConvertAsync(input).Result;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test, TestCaseSource(typeof(ConverterServiceTests), nameof(ShouldThrowFaultExceptionTestCases))]
        public void ConvertAsync_ThrowsFaultException_WhenInputHasWrongFormat(string input)
        {
            // Act and Assert
            Assert.ThrowsAsync<FaultException<NumberFormatFault>>(async () => await _converterService.ConvertAsync(input));
        }

        public static IEnumerable ShouldReturnCorrectResultTestCases
        {
            get
            {
                yield return new TestCaseData("0", "zero dollars");
                yield return new TestCaseData("1", "one dollar");
                yield return new TestCaseData("25,10", "twenty-five dollars and ten cents");
                yield return new TestCaseData("0,01", "zero dollars and one cent");
                yield return new TestCaseData("45 100", "forty-five thousand one hundred dollars");
                yield return new TestCaseData("999 999 999,99", 
                    "nine hundred ninety-nine million nine hundred ninety-nine thousand nine hundred ninety-nine dollars and ninety-nine cents");
            }
        }

        public static IEnumerable ShouldThrowFaultExceptionTestCases
        {
            get
            {
                yield return new TestCaseData("");
                yield return new TestCaseData(string.Empty);
                yield return new TestCaseData("99.99");
                yield return new TestCaseData("99,,99");
                yield return new TestCaseData("99,9");
                yield return new TestCaseData(",99");
                yield return new TestCaseData("a,99");
                yield return new TestCaseData("99,999");
                yield return new TestCaseData("9999999999,99");
            }
        }
    }
}
