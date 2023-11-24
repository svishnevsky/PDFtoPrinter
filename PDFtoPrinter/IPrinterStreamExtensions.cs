using System;
using System.IO;
using System.Threading.Tasks;

namespace PDFtoPrinter
{
    public static class IPrinterStreamExtensions
    {
        /// <summary>
        /// Stores <paramref name="stream"/> to the temp folder and run printer.
        /// </summary>
        /// <param name="printer">Printer instance.</param>
        /// <param name="stream">File stream to be printed.</param>
        /// <param name="options">Options to a printer util.</param>
        /// <param name="timeout">
        /// Printing timeout. If PDFtoPrinter_m.exe process isn't exited after this timeout, 
        /// the process will be killed. Default value is 1 minute.
        /// </param>
        /// <returns>Asynchronous task.</returns>
        public static async Task Print(
            this IPrinter printer,
            Stream stream,
            StreamPrintingOptions options,
            TimeSpan? timeout = null)
        {
            stream.Seek(stream.Position, SeekOrigin.Begin);
            using (FileStream writer = File.OpenWrite(options.FilePath))
            {
                await stream.CopyToAsync(writer).ConfigureAwait(false);
            }

            await printer.Print(options, timeout).ConfigureAwait(false);
        }
    }
}
