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

    [HttpPost("{printerName}/PrintFormFile")]
    public async Task<bool> PrintLabelFromFormAsync(string printerName)
    {
        if (printerName.Contains(' '))
        {
            return false;
        }
        IFormCollection form = await this.HttpContext.Request.ReadFormAsync(this.HttpContext.RequestAborted);
        if (form.Files.Count != 1)
        {
            return false;
        }

        string filePath = Path.GetTempFileName();
        Console.WriteLine($"Saving file to {filePath}");
        using (Stream stream = form.Files[0].OpenReadStream())
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await stream.CopyToAsync(fileStream);
        }

        var printer = new PDFtoPrinterPrinter();
        await printer.Print(new PrintingOptions(printerName, filePath));

        return true;
    }
}
