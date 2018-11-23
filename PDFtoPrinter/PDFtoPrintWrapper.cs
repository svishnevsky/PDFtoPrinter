using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace PDFtoPrinter
{
    public class PDFtoPrintWrapper
    {
        private static readonly string utilPath = Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), 
            "PDFtoPrinter.exe");
        private static readonly TimeSpan printTimeout = new TimeSpan(0, 1, 0);

        /// <summary>
        /// Runs new PDFtoPrinter.exe process with passed parameters
        /// </summary>
        /// <param name="filePath">Path to a PDF file.</param>
        /// <param name="printerName">Name of a printer (if the printer is network, use network format e.g. "\\printmachine\defaultprinter").</param>
        /// <param name="timeount">Printing timeout. If PDFtoPrinter.exe process isn't exited after this timeout, the process will be killed. Default value is 1 minute.</param>
        /// <returns>Asynchronous task.</returns>
        public async Task Print(
            string filePath, string printerName, TimeSpan? timeount = null)
        {
            using (var proc = new Process
            {
                StartInfo =
                    {
                    WindowStyle = ProcessWindowStyle.Hidden,
                        FileName = utilPath,
                        Arguments = $@"""{filePath}"" ""{printerName}""",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
            })
            {
                proc.Start();
                bool result = await proc.WaitForExitAsync(timeount ?? printTimeout);
                if (!result)
                {                    
                    proc.Kill();
                }
            }
        }
    }
}