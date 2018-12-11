using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PDFtoPrinter
{
    public static class ProcessExtentions
    {
        public static async Task<bool> WaitForExitAsync(
            this Process process, TimeSpan timeout)
        {
            return await Task.Factory.StartNew(
                () => process.WaitForExit((int)timeout.TotalMilliseconds))
                .ConfigureAwait(false);
        }
    }
}