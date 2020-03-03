using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PDFtoPrinter.Tests
{
    [TestClass]
    public class StringExtensionsTests
    {
        [TestMethod]
        public void WhenInputNull_ThenReturnNull()
        {
            string input = null;

            string result = input.Format(x => $"formatted {x}");

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void WhenInputEmpty_ThenReturnEmpty()
        {
            string input = string.Empty;

            string result = input.Format(x => $"formatted {x}");

            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void WhenInputNotEmpty_ThenReturnFormatted()
        {
            string input = "some string";

            string result = input.Format(x => $"formatted {x}");

            Assert.AreEqual($"formatted {input}", result);
        }
    }
}
