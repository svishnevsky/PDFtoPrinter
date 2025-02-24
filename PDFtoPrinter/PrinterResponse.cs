namespace PDFtoPrinter
{
    public sealed class PrinterResponse
        {
            public string Name { get; }

            public PrinterResponse(string name)
            {
                this.Name = name;
            }
        }
    }
