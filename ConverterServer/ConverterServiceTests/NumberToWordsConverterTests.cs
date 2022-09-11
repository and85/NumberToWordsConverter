using System;
using NUnit.Framework;
using AndriiCo.ConverterServer.ConverterServiceLib.Converters;

namespace AndriiCo.ConverterServer.ConverterServiceTests
{
    /// <summary>
    /// Tests for <see cref="NumberToWordsConverter"/>
    /// </summary>
    [TestFixture]
    public class NumberToWordsConverterTests
    {
        private NumberToWordsConverter _converter;

        [OneTimeSetUp]
        public void Init()
        {
            _converter = new NumberToWordsConverter();
        }

        [Test]
        [TestCase(0, "zero")]
        [TestCase(1, "one")]
        [TestCase(10, "ten")]
        [TestCase(11, "eleven")]
        [TestCase(22, "twenty-two")]
        [TestCase(100, "one hundred")]
        [TestCase(101, "one hundred one")]
        [TestCase(1000, "one thousand")]
        [TestCase(1001, "one thousand one")]
        [TestCase(1000000, "one million")]
        [TestCase(1000001, "one million one")]
        [TestCase(1000010, "one million ten")]
        [TestCase(999999999, "nine hundred ninety-nine million nine hundred ninety-nine thousand nine hundred ninety-nine")]
        public void Convert_ReturnsCorrectResult(int number, string expected)
        {
            // Act 
            var actual = _converter.Convert(number);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Convert_ThrowsAnException_WhenNumberIsLessThanZero()
        {
            // Act and Assert
            Assert.Throws<ArgumentException>(() => _converter.Convert(-1));
        }

        [Test]
        public void Convert_ThrowsAnException_WhenNumberIsTooBig()
        {
            // Act and Assert
            Assert.Throws<ArgumentException>(() => _converter.Convert(int.MaxValue));
        }
    }
}
