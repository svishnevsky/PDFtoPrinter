using System;
using System.Threading.Tasks;

namespace PDFtoPrinter
{
    public interface IPDFtoPrinterWrapper
    {
        Task Print(string filePath, string printerName, TimeSpan? timeout = null);
    }
}
