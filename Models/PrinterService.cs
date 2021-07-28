using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Platform;
using Point = System.Drawing.Point;

namespace abit_queue_printer_driver.Models
{
    class PrinterService
    {
        string number = "";
        string phrase;
        string printerName;
        public PrinterService(string printerName)
        {
            this.printerName = printerName;
        }

        public static List<string> GetPrinters()
        {
            return PrinterSettings.InstalledPrinters.Cast<string>().OrderBy(x => x).ToList();
        }
        public async Task Print(string number, string phrase= "Пусть умолкнет \n                всякий критик")
        {
            this.number = number;
            this.phrase = phrase;
            PrintDocument printDoc = new PrintDocument();
            printDoc.DocumentName = "IASA Queue #" + number;
            printDoc.PrintPage += PrintPageHandler;
            // printDoc.PrintController = new StandardPrintController();
            ;
            printDoc.PrinterSettings.PrinterName = printerName;
            Console.WriteLine("Print");
            printDoc.Print();

            //--------------------------------------

            //Uncomment if debugging - shows dialog instead
            // PrintDialog printPrvDlg = new PrintDialog();
            // printPrvDlg.Document = printDoc;
            // printPrvDlg.Width = 228;
            // printPrvDlg.Height = 500;
            // printPrvDlg.ShowDialog();

            //--------------------------------------

            printDoc.Dispose();
        }

        void PrintPageHandler(object sender, PrintPageEventArgs ev)
        {
            try
            {
                //PrivateFontCollection pfc = new PrivateFontCollection();
                //pfc.AddFontFile("BebasNeue.otf");
                FontFamily bebas = new FontFamily("Arial");
                Point center = new Point(0, 10);
                SizeF z = new SizeF();
                Font font;

                z = new SizeF(70, 70);
                center.X = (int) ((ev.PageBounds.Width - z.Width - ev.MarginBounds.Width) / 2);
                ev.Graphics.DrawImage(new Bitmap (AvaloniaLocator.Current.GetService <IAssetLoader> ()
                    .Open (new Uri ($"avares://abit-queue-printer-driver/Assets/logo-b.png"))), center.X, center.Y, z.Width, z.Height);

                center.Y += (int) (1.3 * z.Height);
                font = new Font(bebas, 14);
                // var congrat = "Поздравляем, вы:";
                var congrat = "Вітаємо, ви:";
                z = ev.Graphics.MeasureString(congrat, font);
                center.X = (int) ((ev.PageBounds.Width - z.Width - ev.MarginBounds.Width) / 2);
                ev.Graphics.DrawString(congrat, font, Brushes.Black, center.X, center.Y);

                center.Y += (int) (1.1 * z.Height);
                font = new Font(bebas, 40, FontStyle.Bold);
                z = ev.Graphics.MeasureString(number, font);
                center.X = (int) ((ev.PageBounds.Width - z.Width - ev.MarginBounds.Width) / 2);
                ev.Graphics.DrawString(number, font, Brushes.Black, center.X, center.Y);

                center.Y += (int) (1.1 * z.Height);
                font = new Font(bebas, 11, FontStyle.Italic);
                z = ev.Graphics.MeasureString(phrase, font);
                //Rectangle rect = new Rectangle(0, center.Y, 200, font.Height * (int) Math.Ceiling(z.Width / 200));
                center.X = (int) ((ev.PageBounds.Width - z.Width - ev.MarginBounds.Width) / 2);
                ev.Graphics.DrawString(phrase, font, Brushes.Black, center.X, center.Y);

                font.Dispose();
                bebas.Dispose();
                //pfc.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }


    }
}