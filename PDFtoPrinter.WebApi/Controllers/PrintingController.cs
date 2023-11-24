using Microsoft.AspNetCore.Mvc;

namespace PDFtoPrinter.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PrintingController : ControllerBase
{
    [HttpPost("Print")]
    public async Task<bool> PrintLabelAsync([FromBody] PdfPrintRequest request)
    {
        var printer = new PDFtoPrinterPrinter();
        await printer.Print(new PrintingOptions(request.PrinterName, request.FileLocation));

        return true;
    }
}
