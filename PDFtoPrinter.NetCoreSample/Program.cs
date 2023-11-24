using System.IO;

namespace PDFtoPrinter.NetCoreSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var wrapper = new PDFtoPrinterPrinter();
            //Print from file
            wrapper
                .Print(new PrintingOptions("Microsoft Print to PDF", "somefile.pdf"))
                .Wait();

            // Print from stream
            wrapper
                .Print(File.OpenRead("somefile.pdf"), new StreamPrintingOptions("Microsoft Print to PDF"))
                .Wait();
        }
    }
}
