using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PDFtoPrinter.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.Printer.Text = "Microsoft Print to PDF";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(this.PrintingCount.Text, out int count))
            {
                return;
            }

            var wrapper = new PDFtoPrinterPrinter(count);
            Task.WaitAll(Enumerable
                .Range(0, count)
                .Select(x => wrapper.Print(new PrintingOptions(
                    this.Printer.Text,
                    "somefile.pdf")))
                .ToArray());
        }
    }
}
