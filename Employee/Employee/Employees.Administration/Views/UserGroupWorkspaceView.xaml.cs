using System.Windows.Controls;
using Employees.Shared.View;

namespace Employees.Administration.Views
{
    public partial class UserGroupWorkspaceView : UserControl, IWorkspaceBaseView
    {
        public UserGroupWorkspaceView()
        {
            InitializeComponent();
        }

        public void ChangedActiveItem()
        {
            documentContainer.SelectedTabIndex = documentContainer.Items.Count - 1;
        }
    }
}