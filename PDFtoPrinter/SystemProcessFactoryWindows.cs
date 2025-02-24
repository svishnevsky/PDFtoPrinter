using System.Diagnostics;

namespace PDFtoPrinter
{
    public partial class SystemProcessFactory
    {
#if WINDOWS
        /// <inheritdoc/>
        public IProcess Create(string executablePath, PrintingOptions options)
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
#endif
    }
}
