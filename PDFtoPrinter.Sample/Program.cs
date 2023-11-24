using System;
using System.Linq;
using System.Threading.Tasks;

namespace PDFtoPrinter.Sample
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var wrapper = new PDFtoPrinterPrinter(5);
            Task.WaitAll(Enumerable
                .Range(0, 7)
                .Select(x => wrapper.Print(new PrintingOptions(
                    "Microsoft Print to PDF",
                    "somefile.pdf")))
                .ToArray());
            Console.ReadKey();
        }
    }
}
