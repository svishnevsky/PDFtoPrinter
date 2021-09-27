using System.Threading.Tasks;

namespace PDFtoPrinter
{
    public class QueuedFile
    {
        public QueuedFile(string filePath)
        {
            this.Path = filePath;
            this.TaskCompletionSource = new TaskCompletionSource<string>();
        }

        public string Path { get; }

        public TaskCompletionSource<string> TaskCompletionSource { get; }
    }
}
