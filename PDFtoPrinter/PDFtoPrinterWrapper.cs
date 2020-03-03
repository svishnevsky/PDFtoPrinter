using System;
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
    public class PDFtoPrinterWrapper : IPDFtoPrinterWrapper
    {
        private const string UtilName = "PDFtoPrinter.exe";
        private static readonly string UtilPath = GetUtilPath(UtilName);
        private static readonly TimeSpan PrintTimeout = new TimeSpan(0, 1, 0);

        private readonly SemaphoreSlim semaphore;
        private readonly IProcessFactory processFactory;

        /// <summary>
        /// Creates new <see cref="PDFtoPrinterWrapper"/> instance without concurrent printing.
        /// <param name="processFactory"><see cref="IProcessFactory"/> instance.</param>
        /// </summary>
        public PDFtoPrinterWrapper(
            IProcessFactory processFactory = null)
            : this(1, processFactory)
        {
        }

        /// <summary>
        /// Creates new <see cref="PDFtoPrinterWrapper"/> instance with concurrent printing.
        /// </summary>
        /// <param name="maxConcurrentPrintings">Max count of cuncurrent printings.</param>
        /// <param name="processFactory"><see cref="IProcessFactory"/> instance.</param>
        /// <exception cref="ArgumentException">
        /// Thows an exception if <paramref name="maxConcurrentPrintings"/> less or equals 0.
        /// </exception>
        public PDFtoPrinterWrapper(
            int maxConcurrentPrintings,
            IProcessFactory processFactory = null)
        {
            if (maxConcurrentPrintings <= 0)
            {
                throw new ArgumentException(
                    ErrorMessages.ValueGreterZero,
                    nameof(maxConcurrentPrintings));
            }

            this.semaphore = new SemaphoreSlim(maxConcurrentPrintings);
            this.processFactory = processFactory ?? new SystemProcessFactory();
        }

        /// <summary>
        /// Runs new PDFtoPrinter.exe process with passed parameters
        /// </summary>
        /// <param name="filePath">Path to a PDF file.</param>
        /// <param name="printerName">
        /// Name of a printer (if the printer is network, use network format e.g. "\\printmachine\defaultprinter").
        /// </param>
        /// <param name="timeout">
        /// Printing timeout. If PDFtoPrinter.exe process isn't exited after this timeout, 
        /// the process will be killed. Default value is 1 minute.
        /// </param>
        /// <returns>Asynchronous task.</returns>
        public async Task Print(
            string filePath, string printerName, TimeSpan? timeout = null)
        {
            await this.semaphore.WaitAsync().ConfigureAwait(false);
            try
            {
                using (IProcess proc = this.CreateProcess(filePath, printerName))
                {
                    proc.Start();
                    bool result = await proc
                        .WaitForExitAsync(timeout ?? PrintTimeout)
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

        private IProcess CreateProcess(string filePath, string printerName)
        {
            return this.processFactory.Create(UtilPath, filePath, printerName);
        }

        private static string GetUtilPath(string utilName)
        {
            return Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                utilName);
        }
    }
}
