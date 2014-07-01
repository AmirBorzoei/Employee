using System.Windows;
using System.Windows.Controls;
using Employees.Shared.Interfaces;

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
            
            TableViewAllEmployees.ShowPrintPreviewDialog(ownerWindow);
        }
    }
}