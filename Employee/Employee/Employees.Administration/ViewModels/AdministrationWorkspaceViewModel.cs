using Caliburn.Micro;
using Employees.Shared.Interfaces;
using Employees.Shared.ViewModels;

namespace Employees.Administration.ViewModels
{
    public interface IAdministrationWorkspaceViewModel : IWorkspaceBase, ISupportNew, ISupportSave, ISupportReload
    {
    }

    public class AdministrationWorkspaceViewModel : WorkspaceBase, IAdministrationWorkspaceViewModel
    {
        private readonly IUserGroupWorkspaceViewModel _userGroupWorkspaceViewModel;
        private readonly IUserViewModel _userViewModel;


        public AdministrationWorkspaceViewModel(IEventAggregator eventAggregator,
                                                IUserViewModel userViewModel,
                                                IUserGroupWorkspaceViewModel userGroupWorkspaceViewModel) : base(eventAggregator)
        {
            _userViewModel = userViewModel;
            _userGroupWorkspaceViewModel = userGroupWorkspaceViewModel;

            DisplayName = "مدیریت";
        }


        protected override void OnInitialize()
        {
            base.OnInitialize();

            Items.Add(_userGroupWorkspaceViewModel);
            Items.Add(_userViewModel);

            ActivateItem(_userGroupWorkspaceViewModel);
        }


        public void New()
        {
            var item = ActiveItem as ISupportNew;
            if (item != null)
                item.New();
        }

        public void Save()
        {
            var item = ActiveItem as ISupportSave;
            if (item != null)
                item.Save();
        }

        public void Reload()
        {
            var item = ActiveItem as ISupportReload;
            if (item != null)
                item.Reload();
        }
    }
}