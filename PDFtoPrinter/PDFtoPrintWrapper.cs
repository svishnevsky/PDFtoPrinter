using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace PDFtoPrinter
{
    /// <summary>
    /// Wrapper over the PDFtoPrinting.exe utility. 
    /// Runs new PDFtoPrinting.exe instance per Print call.
    /// </summary>
    public class PDFtoPrintWrapper
    {
        private static readonly string utilPath = GetUtilPath();

        private static readonly TimeSpan printTimeout = new TimeSpan(0, 1, 0);
        private readonly SemaphoreSlim semaphore;

        /// <summary>
        /// Creates new <see cref="PDFtoPrintWrapper"/> instance without concurrent printing.
        /// </summary>
        public PDFtoPrintWrapper()
            : this(1)
        {
        }

        /// <summary>
        /// Creates new <see cref="PDFtoPrintWrapper"/> instance with concurrent printing.
        /// </summary>
        /// <param name="maxConcurrentPrintings">Max count of cuncurrent printings.</param>
        public PDFtoPrintWrapper(int maxConcurrentPrintings)
        {
            if (maxConcurrentPrintings <= 0)
            {
                throw new ArgumentException(
                    ErrorMessages.ValueGreterZero,
                    nameof(maxConcurrentPrintings));
            }

            this.semaphore = new SemaphoreSlim(maxConcurrentPrintings);
        }

        /// <summary>
        /// Runs new PDFtoPrinter.exe process with passed parameters
        /// </summary>
        /// <param name="filePath">Path to a PDF file.</param>
        /// <param name="printerName">Name of a printer (if the printer is network, use network format e.g. "\\printmachine\defaultprinter").</param>
        /// <param name="timeout">Printing timeout. If PDFtoPrinter.exe process isn't exited after this timeout, the process will be killed. Default value is 1 minute.</param>
        /// <returns>Asynchronous task.</returns>
        public async Task Print(
            string filePath, string printerName, TimeSpan? timeout = null)
        {
            await this.semaphore.WaitAsync()
                .ConfigureAwait(false);
            try
            {
                using (var proc = CreateProcess(filePath, printerName))
                {
                    proc.Start();
                    bool result = await proc.WaitForExitAsync(timeout ?? printTimeout)
                        .ConfigureAwait(false);
                    if (!result)
                    {
                        proc.Kill();
                    }
                }
            }
            finally
            {
                this.semaphore.Release();
            }
        }

        private static Process CreateProcess(string filePath, string printerName)
        {
            return new Process
            {
                StartInfo =
                    {
                    WindowStyle = ProcessWindowStyle.Hidden,
                        FileName = utilPath,
                        Arguments = $@"""{filePath}"" ""{printerName}""",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
            };
        }

        private static string GetUtilPath()
        {
            return Path.Combine(
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                        "PDFtoPrinter.exe");
        }
    }
}