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
    public class PDFtoPrinterPrinter : IPrinter
    {
        private const string UtilName = "PDFtoPrinter_m.exe";
        private static readonly string utilPath = GetUtilPath(UtilName);
        private static readonly TimeSpan printTimeout = new TimeSpan(0, 1, 0);

        private readonly SemaphoreSlim semaphore;
        private readonly IProcessFactory processFactory;

        /// <summary>
        /// Creates new <see cref="PDFtoPrinterPrinter"/> instance without concurrent printing.
        /// <param name="processFactory"><see cref="IProcessFactory"/> instance.</param>
        /// </summary>
        public PDFtoPrinterPrinter(
            IProcessFactory processFactory = null)
            : this(1, processFactory)
        {
        }

        /// <summary>
        /// Creates new <see cref="PDFtoPrinterPrinter"/> instance with concurrent printing.
        /// </summary>
        /// <param name="maxConcurrentPrintings">Max count of concurrent printings.</param>
        /// <param name="processFactory"><see cref="IProcessFactory"/> instance.</param>
        /// <exception cref="ArgumentException">
        /// Throws an exception if <paramref name="maxConcurrentPrintings"/> less or equals 0.
        /// </exception>
        public PDFtoPrinterPrinter(
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

        [Obsolete("Please use \"Task Print(PrintingOptions options, TimeSpan? timeout = null)\" method instead.")]
        public Task Print(
            string filePath, string printerName, TimeSpan? timeout = null)
        {
            return this.Print(
                new PrintingOptions(printerName, filePath),
                timeout);
        }

        /// <inheritdoc/>
        public async Task Print(PrintingOptions options, TimeSpan? timeout = null)
        {
            await this.semaphore.WaitAsync().ConfigureAwait(false);
            try
            {
                using (IProcess proc = this.processFactory.Create(utilPath, options))
                {
                    proc.Start();
                    bool result = proc.WaitForExit((int)(timeout ?? printTimeout).TotalMilliseconds);
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

        private static string GetUtilPath(string utilName)
        {
            // Fallback logic is required to support all CLR versions.
            string utilLocation = Path.Combine(AppContext.BaseDirectory, utilName);

            return File.Exists(utilLocation)
                ? utilLocation
                : Path.Combine(
                    Path.GetDirectoryName((Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly()).Location),
                    utilName);
        }
    }
}
