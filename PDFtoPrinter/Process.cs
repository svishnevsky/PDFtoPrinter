using System;
using System.Threading.Tasks;

namespace PDFtoPrinter
{
    /// <summary>
    /// System Process successor that extracts an interface.
    /// </summary>
    public class Process :
        System.Diagnostics.Process,
        IProcess
    {
        /// <inheritdoc/>
        public Task<bool> WaitForExitAsync(TimeSpan timeout)
        {
            return Task.Factory.StartNew(
                () => this.WaitForExit((int)timeout.TotalMilliseconds));
        }
    }
}
