namespace PDFtoPrinter.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var wrapper = new PDFtoPrintWrapper();
            wrapper.Print("somefile.pdf", "Microsoft Print to PDF").Wait();
        }
    }
}
