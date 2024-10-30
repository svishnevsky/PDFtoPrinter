using System.Diagnostics;

namespace PDFtoPrinter
{
    /// <inheritdoc/>
    public class SystemProcessFactory : IProcessFactory
    {
        /// <inheritdoc/>
        public IProcess Create(string executablePath, PrintingOptions options)
        {
#if WINDOWS
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
#else
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
#endif
    }
}
