using System;
using System.Threading.Tasks;

namespace PDFtoPrinter
{
    /// <summary>
    /// Describes Process API.
    /// </summary>
    public interface IProcess : IDisposable
    {
        /// <summary>
        /// Starts the process.
        /// </summary>
        /// <returns>
        /// true if a process resource is started; false if no new process resource is started
        /// </returns>
        bool Start();

        /// <summary>
        /// Kills the process.
        /// </summary>
        void Kill();

        /// <summary>
        /// Waits until the process is completed.
        /// </summary>
        /// <param name="timeout">Wait timeout.</param>
        /// <returns>true if the process completed, false otherwise.</returns>
        Task<bool> WaitForExitAsync(TimeSpan timeout);
    }
}
