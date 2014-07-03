using System.Windows;
using System.Windows.Controls;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting;
using Employees.Shared.Interfaces;
using PaperKind = System.Drawing.Printing.PaperKind;

namespace Employees.Personally.Views
{
    /// <summary>
    ///     Interaction logic for EmployeeInfoView.xaml
    /// </summary>
    public partial class EmployeeInfoView : UserControl, ISupportPrint
    {
        public EmployeeInfoView()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            TableViewAllEmployees.ShowPrintPreview(this);
        }

        public void Print(Window ownerWindow)
        {
            //TableViewAllEmployees.ShowPrintPreviewDialog(ownerWindow);

            using (var print = new PrintableControlLink(TableViewAllEmployees))
            {
                print.PrintingSystem.PageSettings.PaperKind = (PaperKind)DevExpress.Xpf.Core.Native.PaperKind.A4;
                var preview = new DocumentPreviewWindow
                {
                    Owner = ownerWindow,
                    Model = new LinkPreviewModel(print)
                };

                print.PrintingSystem.PageSettings.Landscape = true;
                print.CreateDocument(true);
                ((IPrintingSystem)print.PrintingSystem).AutoFitToPagesWidth = 1;
                print.PrintingSystem.ShowPrintStatusDialog = true;


                preview.FlowDirection = FlowDirection.RightToLeft;


                preview.Show();
            }
        }
    }
}