# PDFtoPrinter

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/DarqueWarrior/generator-team/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/dt/PDFtoPrinter.svg)](https://www.nuget.org/packages/PDFtoPrinter/)
[![Build status](https://vishnevsky.visualstudio.com/PDFtoPrinter/_apis/build/status/PDFtoPrinter%20Build)](https://vishnevsky.visualstudio.com/PDFtoPrinter/_build/latest?definitionId=1)

The PDFtoPrinter project Allows to print PDF files uses [PDFtoPrinter](http://www.columbia.edu/~em36/pdftoprinter.html) util. The package contains PDFtoPrinter.exe and copys it to the output folder before build event. Also it provides PDFtoPrintWrapper class that runs PDFtoPrinter.exe inside of a "Print" method in a separate process with default timeout 1 minute (the timeout can be overrited by 3rd argument).

Sample usage:

```C#
var printWrapper = new PDFtoPrintWrapper();
printWrapper.Print("c:\path\to\pdf\file.pdf", "Vendor Color Printer Name");
```

or

```C#
var printWrapper = new PDFtoPrintWrapper();
printWrapper.Print("c:\path\to\pdf\file.pdf", "\\myprintserver\printer1", new TimeSpan(0, 30, 0));
```