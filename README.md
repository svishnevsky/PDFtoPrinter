# PDFtoPrinter

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/DarqueWarrior/generator-team/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PDFtoPrinter.svg)](https://www.nuget.org/packages/PDFtoPrinter/)
[![NuGet](https://img.shields.io/nuget/dt/PDFtoPrinter.svg)](https://www.nuget.org/packages/PDFtoPrinter/)
[![Build status](https://vishnevsky.visualstudio.com/PDFtoPrinter/_apis/build/status/PDFtoPrinter%20Build)](https://vishnevsky.visualstudio.com/PDFtoPrinter/_build/latest?definitionId=1)

The PDFtoPrinter project Allows to print PDF files uses [PDFtoPrinter](http://www.columbia.edu/~em36/pdftoprinter.html) util. The package contains PDFtoPrinter_m.exe and copys it to the output folder before build event. Also it provides PDFtoPrinterPrinter class that runs PDFtoPrinter_m.exe inside of a "Print" method in a separate process with default timeout 1 minute (the timeout can be overrited by 3rd argument). The "Print" method runs new PDFtoPrinter_m.exe instance per call. By default new printing will not start while the previous from the same PDFtoPrinterPrinter instance isn't completed. But you can use set concurrency level using constructor with arguments. 

*Note: Concurrency level works inside PDFtoPrinterPrinter instance.*

Sample usage:

Use local printer
```C#
var filePath = @"c:\path\to\pdf\file.pdf";
var printerName = "Vendor Color Printer Name";
var printer = new PDFtoPrinterPrinter();
printer.Print(new PrintingOptions(printerName, filePath));
```

Use network printer with timeout
```C#
var filePath = @"c:\path\to\pdf\file.pdf";
var networkPrinterName = @"\\myprintserver\printer1";
var printTimeout = new TimeSpan(0, 30, 0);
var printer = new PDFtoPrinterPrinter();
printer.Print(new PrintingOptions(networkPrinterName, filePath), printTimeout);
```

Use network printer with 5 concurrency printings. In this case up to 5 instances of PDFtoPrinter_m.exe will be started simultaneously
```C#
var filePath = @"c:\path\to\pdf\file.pdf";
var networkPrinterName = @"\\myprintserver\printer1";
var allowedCocurrentPrintings = 5;
var printer = new PDFtoPrinterPrinter(allowedCocurrentPrintings);
for (var i = 0; i < 10; i++)
{
    wrapper.Print(new PrintingOptions(networkPrinterName, filePath));
}
```

If you need to delete files after printing you can use "CleanupFilesPrinter":
```C#
var filePath = @"c:\path\to\pdf\file.pdf";
var networkPrinterName = @"\\myprintserver\printer1";
var printer = new CleanupFilesPrinter(new PDFtoPrinterPrinter());
printer.Print(new PrintingOptions(networkPrinterName, filePath));
```

## dotnet support

*PDFToPrinter* package is available on Windows machines only. 
If an application references *net5.0* framework and later then it is required to change "TargetFramework"
to *net[version].0-windows* in a *csproj* file. E.g.
```
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net7.0-windows</TargetFramework>
  </PropertyGroup>

</Project>
```
