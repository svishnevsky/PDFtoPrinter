using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace PDFtoPrinter
{
    /// <inheritdoc/>
    public class SystemProcessFactory : IProcessFactory
    {
        /// <inheritdoc/>
        public IProcess Create(string executablePath, PrintingOptions options)
        {
#if !NET45_OR_GREATER && !NET48_OR_GREATER
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && Environment.GetEnvironmentVariable("USE_LP") != "true")
#endif
            {
                return new Process
                {
                    StartInfo =
                    {
                        WindowStyle = ProcessWindowStyle.Hidden,
                        FileName = executablePath,
                        Arguments = $"{options} /s",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
            }
            string args = "";
            if (!string.IsNullOrWhiteSpace(options.PrinterName))
            {
                args += $" -d {options.PrinterName}";
            }
            if (!string.IsNullOrWhiteSpace(options.Pages))
            {
                args += $" -P {options.Pages}";
            }
            if (options.Copies.HasValue)
            {
                args += $" -c {options.Copies}";
            }
            if (!string.IsNullOrWhiteSpace(options.Focus))
            {
                args += $" -t {options.Focus}";
            }
            args += $" {options.FilePath}";
            args = args.Trim();

            return new Process
            {
                StartInfo =
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = executablePath,
                    Arguments = args,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
        }
    }
}
