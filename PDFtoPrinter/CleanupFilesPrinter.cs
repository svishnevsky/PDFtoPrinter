using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Printing;
using System.Threading.Tasks;
using System.Timers;

namespace PDFtoPrinter
{
    /// <summary>
    /// Deletes files after printing. Doesn't print files by it selves but use inner printer.
    /// </summary>
    public class CleanupFilesPrinter : IPrinter
    {
        public delegate void OnCleanupFailedHandler(QueuedFile file, Exception exception);
        public static event OnCleanupFailedHandler OnCleanupFailed;
        private static readonly IDictionary<string, ConcurrentQueue<QueuedFile>> printingQueues =
            new ConcurrentDictionary<string, ConcurrentQueue<QueuedFile>>();
        private static readonly object locker = new object();
        private static readonly Timer cleanupTimer = new Timer(1 * 1000)
        {
            AutoReset = true,
            Enabled = true
        };
        private static bool deletingInProgress = false;

        private readonly IPrinter inner;
        private readonly bool waitFileDeletion;

        static CleanupFilesPrinter()
        {
            cleanupTimer.Elapsed += CleanupTimerElapsed;
            cleanupTimer.Enabled = true;
        }


        /// <summary>
        /// Creates new <see cref="CleanupFilesPrinter"/> instance.
        /// </summary>
        /// <param name="inner">The inner printer that will print files.</param>
        /// <param name="waitFileDeletion">If "true" "Print" method will wait until a file is deleted.
        /// The file will be deleted in background otherwise</param>
        public CleanupFilesPrinter(
            IPrinter inner,
            bool waitFileDeletion = false)
        {
            this.inner = inner;
            this.waitFileDeletion = waitFileDeletion;
        }

        /// <inheritdoc/>
        public async Task Print(PrintingOptions options, TimeSpan? timeout = null)
        {
            await this.inner.Print(options, timeout);
            Task task = EnqueuePrintingFile(options.PrinterName, options.FilePath);
            if (this.waitFileDeletion)
            {
                await task;
            }
        }

        private static Task EnqueuePrintingFile(string printerName, string filePath)
        {
            ConcurrentQueue<QueuedFile> queue = GetQueue(printerName);
            var file = new QueuedFile(filePath);
            queue.Enqueue(file);

            return file.TaskCompletionSource.Task;
        }

        private static ConcurrentQueue<QueuedFile> GetQueue(string printerName)
        {
            if (!printingQueues.ContainsKey(printerName))
            {
                lock (locker)
                {
                    if (!printingQueues.ContainsKey(printerName))
                    {
                        printingQueues.Add(printerName, new ConcurrentQueue<QueuedFile>());
                    }
                }
            }

            return printingQueues[printerName];
        }

        private static void CleanupTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (deletingInProgress)
            {
                return;
            }

            deletingInProgress = true;
            CleanupPrintedFiles(printingQueues);
            deletingInProgress = false;
        }

        private static void CleanupPrintedFiles(
            IDictionary<string, ConcurrentQueue<QueuedFile>> printingQueues)
        {
            using (var printServer = new PrintServer())
            {
                foreach (KeyValuePair<string, ConcurrentQueue<QueuedFile>> queue
                    in printingQueues)
                {
                    DeletePrintedFiles(printServer, queue.Key, queue.Value);
                }
            }
        }

        private static void DeletePrintedFiles(
            PrintServer printServer,
            string queueName,
            ConcurrentQueue<QueuedFile> files)
        {
            using (PrintQueue printerQueue = printServer.GetPrintQueue(queueName))
            {
                DeletePrintedFiles(files, printerQueue);
            }
        }

        private static void DeletePrintedFiles(
            ConcurrentQueue<QueuedFile> files,
            PrintQueue printerQueue)
        {
            var printingItems = new HashSet<string>(printerQueue
                .GetPrintJobInfoCollection()
                .Select(x => x.Name.ToUpper()));
            while (!files.IsEmpty)
            {
                files.TryPeek(out QueuedFile currentFile);
                if (printingItems.Contains(Path.GetFileName(currentFile.Path).ToUpper()))
                {
                    break;
                }

                files.TryDequeue(out QueuedFile dequeuedFile);
                try
                {
                    File.Delete(dequeuedFile.Path);
                    dequeuedFile.TaskCompletionSource.SetResult(dequeuedFile.Path);
                }
                catch (Exception ex)
                {
                    OnCleanupFailed?.Invoke(dequeuedFile, ex);
                }
            }
        }
    }
}
