namespace PDFtoPrinter.Sample
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var wrapper = new PDFtoPrinterWrapper();
            wrapper.Print("somefile.pdf", "Microsoft Print to PDF").Wait();
        }
    }
}
