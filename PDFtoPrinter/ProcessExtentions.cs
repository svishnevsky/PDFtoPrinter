using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace PDFtoPrinter
{
    public static class ProcessExtentions
    {
        public static Task<bool> WaitForExitAsync(this Process process, TimeSpan timeout)
        {
            var processWaitObject = new ManualResetEvent(false)
            {
                SafeWaitHandle = new SafeWaitHandle(process.Handle, false)
            };
            var tcs = new TaskCompletionSource<bool>();
            RegisteredWaitHandle registeredProcessWaitHandle = null;
            registeredProcessWaitHandle = ThreadPool.RegisterWaitForSingleObject(
                processWaitObject,
                delegate (object state, bool timedOut)
                {
                    if (!timedOut)
                    {
                        registeredProcessWaitHandle?.Unregister(null);
                    }

                    processWaitObject.Dispose();
                    tcs.SetResult(!timedOut);
                },
                null,
                timeout,
                true);

            return tcs.Task;
        }
    }
}