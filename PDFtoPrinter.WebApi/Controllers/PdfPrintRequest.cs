namespace PDFtoPrinter.WebApi.Controllers;

public class PdfPrintRequest
{
    public required string PrinterName { get; set; }

    public required string FileLocation { get; set; }
}
