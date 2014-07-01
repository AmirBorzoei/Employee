using System.Threading.Tasks;
using Caliburn.Micro;
using Employees.DAL.Criteria;
using Employees.DAL.Entities;
using Employees.DAL.Repositories;
using Employees.Shared.Interfaces;
using Employees.Shared.Models;
using Employees.Shared.Services;
using Employees.Shared.ViewModels;

namespace Employees.Administration.ViewModels
{
    public interface IUserViewModel : IScreenBase, ISupportSave, ISupportReload
    {
    }

    public class UserViewModel : ScreenBase<User>, IUserViewModel
    {
        private readonly IEmployeeUnitOfWork _employeeUnitOfWork;


        public UserViewModel(IEmployeeUnitOfWork employeeUnitOfWork)
        {
            _employeeUnitOfWork = employeeUnitOfWork;

            DisplayName = "کاربران";

            Users = new BindableCollection<User>();
            UserGroups = new BindableCollection<UserGroup>();
        }


        public BindableCollection<User> Users { get; private set; }
        public BindableCollection<UserGroup> UserGroups { get; private set; }


        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            Reload();
        }


        public void Save()
        {
            Task.Factory.StartNew(ProgressBarService.Show);

            var task = Task.Factory.StartNew(() =>
            {
                foreach (var user in Users)
                {
                    if (user.IsDirty)
                        _employeeUnitOfWork.UserRepository.UpdateOrInsert(user);
                }

                LoadUsers();
            });

            task.ContinueWith(arg => Task.Factory.StartNew(ProgressBarService.Hide));
        }

        public void Reload()
        {
            Task.Factory.StartNew(ProgressBarService.Show);

            var task = Task.Factory.StartNew(() =>
            {
                LoadUsers();
                LoadUserGroups();
            });

            task.ContinueWith(arg => Task.Factory.StartNew(ProgressBarService.Hide));
        }


        private void LoadUsers()
        {
            Users.Clear();

            var searchQuery = new SearchQuery<UserEntity>();
            searchQuery.IncludeProperties = "UserGroups";

            Users.AddRange(_employeeUnitOfWork.UserRepository.GetUsers(searchQuery));
        }

        private void LoadUserGroups()
        {
            UserGroups.Clear();
            UserGroups.AddRange(_employeeUnitOfWork.UserGroupRepository.GetUserGroups());
        }
    }
}