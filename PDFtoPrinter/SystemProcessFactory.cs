using System.Diagnostics;

namespace PDFtoPrinter
{
    /// <inheritdoc/>
    public class SystemProcessFactory : IProcessFactory
    {
        /// <inheritdoc/>
        public IProcess Create(string executablePath, PrintingOptions options)
        {
            return new Process
            {
                StartInfo =
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = executablePath,
                    Arguments = options.ToString(),
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
        }
    }
}
