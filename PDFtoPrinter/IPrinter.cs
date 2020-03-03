using System;
using System.Threading.Tasks;

namespace PDFtoPrinter
{
    public interface IPrinter
    {
        /// <summary>
        /// Runs new PDFtoPrinter.exe process with passed parameters
        /// </summary>
        /// <param name="options">Options to a printer util.</param>
        /// <param name="timeout">
        /// Printing timeout. If PDFtoPrinter.exe process isn't exited after this timeout, 
        /// the process will be killed. Default value is 1 minute.
        /// </param>
        /// <returns>Asynchronous task.</returns>
        Task Print(PrintingOptions options, TimeSpan? timeout = null);
    }
}
