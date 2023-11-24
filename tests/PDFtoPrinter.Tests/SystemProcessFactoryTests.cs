using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PDFtoPrinter.Tests;

[TestClass]
public class SystemProcessFactoryTests
{
    [TestMethod]
    public void WhenParametersValid_ThenCreateProcessInstance()
    {
        const string ExecutablePath = "pathto.exe";
        const string PrinterName = "printer";
        const string FilePath = "file.path";
        var options = new PrintingOptions(PrinterName, FilePath);
        var factory = new SystemProcessFactory();

        var process = (Process)factory.Create(ExecutablePath, options);

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
            $"\"{FilePath}\" \"{PrinterName}\" /s");
    }
}
