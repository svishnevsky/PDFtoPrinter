using System;

namespace PDFtoPrinter
{
    [Obsolete("Please use \"PDFtoPrinterPrinter\" instead.")]
    public class PDFtoPrintWrapper : PDFtoPrinterPrinter
    {
        public PDFtoPrintWrapper(
            IProcessFactory processFactory = null)
            : base(processFactory)
        {
        }

        public PDFtoPrintWrapper(
            int maxConcurrentPrintings,
            IProcessFactory processFactory = null)
            : base(maxConcurrentPrintings, processFactory)
        {
        }
    }
}
