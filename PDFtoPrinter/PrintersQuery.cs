using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace PDFtoPrinter
{
    public sealed class PrintersQuery
    {
        public static async Task<PrinterResponse[]> RunAsync()
        {
#if NET45_OR_GREATER && !NET48_OR_GREATER
            // until a better solution is found for net45 and net46
            throw new NotImplementedException();
#else
            var printers = new List<PrinterResponse>();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var wmb = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");
                foreach (ManagementBaseObject printer in wmb.Get())
                {
                    printers.Add(new PrinterResponse(printer["Name"]?.ToString() ?? string.Empty));
                }
                return printers.Where(x => !string.IsNullOrWhiteSpace(x.Name)).ToArray();
            }
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "lpstat",
                    Arguments = "-a",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.Start();

            string line;
            while ((line = await process.StandardOutput.ReadLineAsync()) != null)
            {
                string[] parts = line.Split(' ');
                if (parts.Length < 2 || string.IsNullOrWhiteSpace(parts[0]))
                {
                    continue;
                }
                printers.Add(new PrinterResponse(parts[0]));
            }

            return printers.ToArray();
#endif
        }
    }

    public sealed class PrinterResponse
    {
        public string Name { get; }

        public PrinterResponse(string name)
        {
            this.Name = name;
        }
    }
}

