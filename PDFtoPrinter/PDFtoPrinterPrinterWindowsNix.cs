namespace PDFtoPrinter
{
    public partial class PDFtoPrinterPrinter
    {
#if !WINDOWS
        private const string UtilName = "lp";

        private static string GetUtilPath(string utilName)
        {
            if (Path.IsPathRooted(utilName))
            {
                return utilName;
            }

            string baseUtilPath = Environment.GetEnvironmentVariable("UTIL_PATH");
            if (!string.IsNullOrEmpty(baseUtilPath))
            {
                return Path.Combine(baseUtilPath, utilName);
            }

            string defaultPath = Path.Combine("/usr/bin", utilName);
            if (File.Exists(defaultPath))
            {
                return defaultPath;
            }

            return utilName;
        }
#endif
    }
}
