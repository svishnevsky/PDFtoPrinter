using System.Diagnostics;
using System.Linq;

namespace PDFtoPrinter
{
    /// <inheritdoc/>
    public class SystemProcessFactory : IProcessFactory
    {
        /// <inheritdoc/>
        public IProcess Create(string executablePath, params string[] args)
        {
            return new Process
            {
                StartInfo =
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = executablePath,
                    Arguments = string.Join(" ", args.Select(x => $"\"{x}\"")),
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
        }
    }
}
