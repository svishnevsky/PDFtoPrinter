using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PDFtoPrinter.Tests
{
    [TestClass]
    public class PrintingOptionsTests
    {
        [DataTestMethod]
        [DataRow("printer", "file.path", null, null, null, false, "\"file.path\" \"printer\"")]
        [DataRow("printer", "file.path", "1,7-9", null, null, false, "\"file.path\" \"printer\" pages=1,7-9")]
        [DataRow("printer", "file.path", null, 12u, null, false, "\"file.path\" \"printer\" copies=12")]
        [DataRow("printer", "file.path", null, null, "win", true, "\"file.path\" \"printer\" focus=\"win\" /csv")]
        [DataRow("printer", "file.path", "7-9", 2u, "win", true, "\"file.path\" \"printer\" pages=7-9 copies=2 focus=\"win\" /csv")]
        public void WhenToStringCalled_ThenReturnFormattedString(
            string printerName,
            string filePath,
            string pages,
            uint? copies,
            string focus,
            bool enableCSV,
            string expected)
        {
            var options = new PrintingOptions(printerName, filePath)
            {
                Copies = copies,
                Focus = focus,
                Pages = pages,
                EnableCSV = enableCSV
            };

            string result = options.ToString();

            Assert.AreEqual(expected, result);
        }
    }
}
