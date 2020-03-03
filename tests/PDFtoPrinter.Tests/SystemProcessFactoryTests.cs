using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PDFtoPrinter.Tests
{
    [TestClass]
    public class SystemProcessFactoryTests
    {
        [TestMethod]
        public void WhenParametersValid_ThenCreateProcessInstance()
        {
            const string ExecutablePath = "pathto.exe";
            string[] args = new[] { "arg1", "arg2" };
            var factory = new SystemProcessFactory();

            var process = (Process)factory.Create(ExecutablePath, args);

            Assert.AreEqual(
                process.StartInfo.WindowStyle, 
                ProcessWindowStyle.Hidden);
            Assert.AreEqual(
                process.StartInfo.UseShellExecute,
                false);
            Assert.AreEqual(
                process.StartInfo.CreateNoWindow,
                true);
            Assert.AreEqual(
                process.StartInfo.FileName,
                ExecutablePath);
            Assert.AreEqual(
                process.StartInfo.Arguments,
                string.Join(" ", args.Select(x => $"\"{x}\"")));
        }
    }
}
