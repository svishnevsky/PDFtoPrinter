using System;

namespace PDFtoPrinter.Sample
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var wrapper = new CleanupFilesPrinter(new PDFtoPrinterPrinter(), true);
            wrapper
                .Print(new PrintingOptions("Microsoft Print to PDF", "somefile.pdf"))
                .Wait();
            Console.ReadKey();
        }
    }
}
