namespace PDFtoPrinter
{
    public partial class PrintersQuery
    {
#if !WINDOWS
        public static async Task<PrinterResponse[]> RunAsync()
        {
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
            var printers = new List<PrinterResponse>();
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
        }
#endif
    }
}
