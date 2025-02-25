using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace PDFtoPrinter
{
    public partial class PrintersQuery
    {
#if WINDOWS && (NETSTANDARD2_0_OR_GREATER || NET6_0_OR_GREATER)
        public static PrinterResponse[] Run()
        {
            var printers = new List<PrinterResponse>();
            var wmb = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");
            foreach (ManagementBaseObject printer in wmb.Get())
            {
                printers.Add(new PrinterResponse(printer["Name"]?.ToString() ?? string.Empty));
            }

            return printers
                .Where(x => !string.IsNullOrWhiteSpace(x.Name))
                .ToArray();
        }
#endif
    }
}
