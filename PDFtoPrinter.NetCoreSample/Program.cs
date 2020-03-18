namespace PDFtoPrinter.NetCoreSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var wrapper = new PDFtoPrinterPrinter();
            wrapper
                .Print(new PrintingOptions("Microsoft Print to PDF", "somefile.pdf"))
                .Wait();
        }
    }
}
