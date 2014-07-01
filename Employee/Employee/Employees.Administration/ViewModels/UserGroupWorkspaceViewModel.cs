using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Input;
using Caliburn.Micro;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.Grid;
using Employees.DAL.Criteria;
using Employees.DAL.Entities;
using Employees.DAL.Repositories;
using Employees.Shared.Interfaces;
using Employees.Shared.Models;
using Employees.Shared.Services;
using Employees.Shared.ViewModels;

namespace Employees.Administration.ViewModels
{
    public interface IUserGroupWorkspaceViewModel : IWorkspaceBase, ISupportNew, ISupportSave, ISupportReload
    {
    }

    public class UserGroupWorkspaceViewModel : WorkspaceBase, IUserGroupWorkspaceViewModel
    {
        private readonly IEmployeeUnitOfWork _employeeUnitOfWork;
        private UserGroup _currentUserGroupSearch;


        public UserGroupWorkspaceViewModel(IEventAggregator eventAggregator,
            IEmployeeUnitOfWork employeeUnitOfWork) : base(eventAggregator)
        {
            _employeeUnitOfWork = employeeUnitOfWork;

            CurrentUserGroupSearch = new UserGroup();
            UserGroups = new BindableCollection<UserGroup>();

            DisplayName = "گروه های کاربری";
        }


        public UserGroup CurrentUserGroupSearch
        {
            get { return _currentUserGroupSearch; }
            set
            {
                _currentUserGroupSearch = value;
                NotifyOfPropertyChange(() => CurrentUserGroupSearch);
            }
        }

        public BindableCollection<UserGroup> UserGroups { get; private set; }


        public void New()
        {
            AddNewTab(new UserGroup());
        }

        public void Save()
        {
            var item = ActiveItem as ISupportSave;
            if (item != null)
                item.Save();

            Reload();
        }

        public void Reload()
        {
            SearchUserGroup();

            var item = ActiveItem as ISupportReload;
            if (item != null)
                item.Reload();
        }

        public void SearchUserGroup()
        {
            Task.Factory.StartNew(ProgressBarService.Show);

            var task = Task.Factory.StartNew(() =>
            {
                UserGroups.Clear();

                var searchQuery = new SearchQuery<UserGroupEntity>();
                if (!string.IsNullOrEmpty(CurrentUserGroupSearch.UserGroupName))
                    searchQuery.AddFilter(ug => ug.UserGroupName.Contains(CurrentUserGroupSearch.UserGroupName));

                UserGroups.AddRange(_employeeUnitOfWork.UserGroupRepository.GetUserGroups(searchQuery));
            });

            task.ContinueWith(arg => Task.Factory.StartNew(ProgressBarService.Hide));
        }

        public void SearchPanelKeyDown(LayoutPanel layoutPanel, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.Key == Key.Return)
            {
                SearchUserGroup();
            }
        }

        public void OpenUserGroup(EditGridCellData editGridCellData)
        {
            if (editGridCellData == null || editGridCellData.RowData == null) return;

            var userGroup = editGridCellData.RowData.Row as UserGroup;
            if (userGroup == null) return;

            userGroup = _employeeUnitOfWork.UserGroupRepository.GetUserGroupFullByID(userGroup.UserGroupId);

            var item = ActiveItem as UserGroupViewModel;
            if (item != null)
                item.CurrentObject = userGroup;
            else
                AddNewTab(userGroup);
        }

        public void OpenUserGroupNewTab(EditGridCellData editGridCellData)
        {
            if (editGridCellData == null || editGridCellData.RowData == null) return;

            var userGroup = editGridCellData.RowData.Row as UserGroup;
            if (userGroup == null) return;

            userGroup = _employeeUnitOfWork.UserGroupRepository.GetUserGroupFullByID(userGroup.UserGroupId);

            AddNewTab(userGroup);
        }

        public void DeleteUserGroup(EditGridCellData editGridCellData)
        {
            if (editGridCellData == null || editGridCellData.RowData == null) return;

            var userGroup = editGridCellData.RowData.Row as UserGroup;
            if (userGroup == null) return;

            _employeeUnitOfWork.UserGroupRepository.DeleteUserGroupById(userGroup.UserGroupId);

            var openDeletedItem = Items.Cast<UserGroupViewModel>().FirstOrDefault(i => i.CurrentObject != null &&
                                                                                       i.CurrentObject.UserGroupId == userGroup.UserGroupId);
            if (openDeletedItem != null)
                Items.Remove(openDeletedItem);
            
            Reload();
        }

        private void AddNewTab(UserGroup userGroup)
        {
            var item = IoC.Get<IUserGroupViewModel>();
            item.CurrentObject = userGroup;

            ActivateItem(item);
        }
    }
}