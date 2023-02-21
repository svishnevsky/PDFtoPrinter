using System.IO;

namespace PDFtoPrinter
{
    /// <summary>
    /// Options for a Printer
    /// </summary>
    public class StreamPrintingOptions : PrintingOptions
    {
        private const string FileExtension = "pdf";

        /// <summary>
        /// Creates new <see cref="StreamPrintingOptions"/> instance.
        /// </summary>
        /// <param name="printerName">Name of the printer.</param>
        public StreamPrintingOptions(string printerName)
            : base(
                printerName,
                Path.ChangeExtension(
                    Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()),
                    FileExtension))
        {
        }
    }
}
