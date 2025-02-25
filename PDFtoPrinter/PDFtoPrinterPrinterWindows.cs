using System;
using System.IO;
using System.Reflection;

namespace PDFtoPrinter
{
    public partial class PDFtoPrinterPrinter
    {
#if WINDOWS
        private const string UtilName = "PDFtoPrinter_m.exe";

        private static string GetUtilPath(string utilName)
        {
            string utilLocation = Path.Combine(AppContext.BaseDirectory, utilName);

            return File.Exists(utilLocation)
                ? utilLocation
                : Path.Combine(
                    Path.GetDirectoryName((Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly()).Location),
                    utilName);
        }
#endif
    }
}
