# PDFtoPrinter

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/DarqueWarrior/generator-team/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PDFtoPrinter.svg)](https://www.nuget.org/packages/PDFtoPrinter/)
[![NuGet](https://img.shields.io/nuget/dt/PDFtoPrinter.svg)](https://www.nuget.org/packages/PDFtoPrinter/)
[![Build status](https://vishnevsky.visualstudio.com/PDFtoPrinter/_apis/build/status/PDFtoPrinter%20Build)](https://vishnevsky.visualstudio.com/PDFtoPrinter/_build/latest?definitionId=1)

The PDFtoPrinter project Allows to print PDF files uses [PDFtoPrinter](http://www.columbia.edu/~em36/pdftoprinter.html) util. The package contains PDFtoPrinter.exe and copys it to the output folder before build event. Also it provides PDFtoPrintWrapper class that runs PDFtoPrinter.exe inside of a "Print" method in a separate process with default timeout 1 minute (the timeout can be overrited by 3rd argument). The "Print" method runs new PDFtoPrinter.exe instance per call. By default new printing will not start while the previous from the same PDFtoPrintWrapper instance isn't completed. But you can use set concurrency level using constructor with arguments. 

*Note: Concurrency level works inside PDFtoPrintWrapper instance.*

Sample usage:

USe local printer
```C#
var filePath = "c:\path\to\pdf\file.pdf";
var printerName = "Vendor Color Printer Name";
var printWrapper = new PDFtoPrintWrapper();
printWrapper.Print(filePath, printerName);
```

Use network printer with timeout
```C#
var filePath = "c:\path\to\pdf\file.pdf";
var networkPrinterName = "\\myprintserver\printer1";
var printTimeout = new TimeSpan(0, 30, 0);
var printWrapper = new PDFtoPrintWrapper();
printWrapper.Print(filePath, networkPrinterName, printTimeout);
```

Use network printer with 5 concurrency printings. In this case up to 5 instances of PDFtoPrinter.exe will be started simultaneously
```C#
var filePath = "c:\path\to\pdf\file.pdf";
var networkPrinterName = "\\myprintserver\printer1";
var allowedCocurrentPrintings = 5;
var printWrapper = new PDFtoPrintWrapper(allowedCocurrentPrintings);
for (var i = 0; i < 10; i++)
{
    wrapper.Print(filePath, networkPrinterName);
}
```