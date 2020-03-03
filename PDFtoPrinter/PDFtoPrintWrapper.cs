using System;

namespace PDFtoPrinter
{
    [Obsolete]
    public class PDFtoPrintWrapper : PDFtoPrinterWrapper
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
