namespace PDFtoPrinter.Sample
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var wrapper = new PDFtoPrinterPrinter();
            wrapper
                .Print(new PrintingOptions("Microsoft Print to PDF", "somefile.pdf"))
                .Wait();
        }
    }
}
