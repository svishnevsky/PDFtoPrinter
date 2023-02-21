using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace PDFtoPrinter.Tests
{
    [TestClass]
    public class IPrinterStreamExtentionsTests
    {
        [TestMethod]
        public async Task WhenCallPrint_ThenCreateTempFile()
        {
            var printer = new Mock<IPrinter>();
            var options = new StreamPrintingOptions("myprinter");
            byte[] fileContent = new byte[] { 1, 2, 3, };
            using (var stream = new MemoryStream(fileContent))
            {
                await printer.Object.Print(stream, options);
            }

            byte[] createdFileContent = File.ReadAllBytes(options.FilePath);

            CollectionAssert.AreEqual(fileContent, createdFileContent);
        }
    }
}
