# PDFtoPrinter

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