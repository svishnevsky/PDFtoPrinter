using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace PDFtoPrinter.Tests
{
    [TestClass]
    public class PDFtoPrinterPrinterTests
    {
        private const string FilePath = "pathto.file";
        private const string PrinterName = "someprinter";

        [DataTestMethod]
        [DataRow(-15)]
        [DataRow(-1)]
        [DataRow(0)]
        public void WhenMaxCuncurrencyInvalid_ThenThrowException(int maxCuncurrency)
        {
            ArgumentException exception = Assert.ThrowsException<ArgumentException>(
                () => new PDFtoPrinterPrinter(maxCuncurrency));
            Assert.AreEqual(
                "Value should be greater than zero. (Parameter 'maxConcurrentPrintings')",
                exception.Message);
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(99)]
        [DataRow(int.MaxValue)]
        public void WhenMaxCuncurrencyValid_ThenCreateInstance(int maxCuncurrency)
        {
            var wrapper = new PDFtoPrinterPrinter(maxCuncurrency);

            Assert.IsNotNull(wrapper);
        }

        [TestMethod]
        public void WhenCallDefaultConstructor_ThenCreateInstance()
        {
            var wrapper = new PDFtoPrinterPrinter();

            Assert.IsNotNull(wrapper);
        }

        [TestMethod]
        public async Task WhenProcessCompleted_ThenDoNotKill()
        {
            var process = new Mock<IProcess>();
            var timeout = TimeSpan.FromMinutes(2);
            process.Setup(x => x.WaitForExitAsync(timeout))
                .ReturnsAsync(true);
            var wrapper = new PDFtoPrinterPrinter(
                CreateProcessFactory(process.Object));

            await wrapper.Print(FilePath, PrinterName, timeout);

            process.Verify(x => x.Start(), Times.Once);
            process.Verify(x => x.Kill(), Times.Never);
        }

        [TestMethod]
        public async Task WhenProcessNotCompleted_ThenKill()
        {
            var process = new Mock<IProcess>();
            process.Setup(x => x.WaitForExitAsync(TimeSpan.FromMinutes(1)))
                .ReturnsAsync(false);
            var wrapper = new PDFtoPrinterPrinter(
                CreateProcessFactory(process.Object));

            await wrapper.Print(new PrintingOptions(PrinterName, FilePath));

            process.Verify(x => x.Start(), Times.Once);
            process.Verify(x => x.Kill(), Times.Once);
        }

        private static IProcessFactory CreateProcessFactory(IProcess process)
        {

            var processFactory = new Mock<IProcessFactory>();
            processFactory
                .Setup(x => x.Create(
                    Path.Combine(
                        Path.GetDirectoryName(
                            typeof(PDFtoPrinterPrinter).Assembly.Location),
                        "PDFtoPrinter_m.exe"),
                    new PrintingOptions(PrinterName, FilePath)))
                .Returns(process);
            return processFactory.Object;
        }
    }
}
