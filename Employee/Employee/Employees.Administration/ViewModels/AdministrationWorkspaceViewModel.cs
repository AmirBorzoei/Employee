using Caliburn.Micro;
using Employees.Shared.Constants;
using Employees.Shared.Interfaces;
using Employees.Shared.Permission;
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

            if (Sission.LoginedUser.HasAccess(PermissionKeys.AdministrationModule_UserGroup, PermissionAccessTypes.Active))
                Items.Add(_userGroupWorkspaceViewModel);
            if (Sission.LoginedUser.HasAccess(PermissionKeys.AdministrationModule_User, PermissionAccessTypes.Active))
                Items.Add(_userViewModel);

            if (Items.Count > 0)
                ActivateItem(Items[0]);
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