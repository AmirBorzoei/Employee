using System.Windows;
using System.Windows.Controls;
using DevExpress.Xpf.Printing;
using Employees.Shared.Interfaces;

namespace Employees.Personally.Views
{
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
                var preview = new DocumentPreviewWindow
                {
                    Owner = ownerWindow,
                    Model = new LinkPreviewModel(print)
                };

                print.CreateDocument(true);

                preview.FlowDirection = FlowDirection.RightToLeft;
                preview.ShowDialog();
            }
        }
    }
}