using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PDFtoPrinter.Tests
{
    [TestClass]
    public class PrintingOptionsTests
    {
        [DataTestMethod]
        [DataRow("printer", "file.path", null, null, null, "\"file.path\" \"printer\"")]
        [DataRow("printer", "file.path", "1,7-9", null, null, "\"file.path\" \"printer\" pages=1,7-9")]
        [DataRow("printer", "file.path", null, 12u, null, "\"file.path\" \"printer\" copies=12")]
        [DataRow("printer", "file.path", null, null, "win", "\"file.path\" \"printer\" focus=\"win\"")]
        [DataRow("printer", "file.path", "7-9", 2u, "win", "\"file.path\" \"printer\" pages=7-9 copies=2 focus=\"win\"")]
        public void WhenToStringCalled_ThenReturnFormattedString(
            string printerName,
            string filePath,
            string pages,
            uint? copies,
            string focus,
            string expected)
        {
            var options = new PrintingOptions(printerName, filePath)
            {
                Copies = copies,
                Focus = focus,
                Pages = pages
            };

            string result = options.ToString();

            Assert.AreEqual(expected, result);
        }
    }
}
