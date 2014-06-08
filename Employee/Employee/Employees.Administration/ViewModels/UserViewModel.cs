using AutoMapper;
using Caliburn.Micro;
using Employees.DAL.Entities;
using Employees.DAL.Repositories;
using Employees.Shared.Constants;
using Employees.Shared.Interfaces;
using Employees.Shared.Models;
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
            
            LoadUserGroups();
        }


        public void Save()
        {
            foreach (var user in Users)
            {
                var userEntity = Mapper.Map<UserEntity>(user);

                if (user.State == ModelStates.New)
                    _employeeUnitOfWork.UserRepository.Insert(userEntity);
                if (user.State == ModelStates.Modified)
                    _employeeUnitOfWork.UserRepository.Update(userEntity);
            }

            LoadUsers();
        }

        public void Reload()
        {
            LoadUsers();
            LoadUserGroups();
        }


        private void LoadUsers()
        {
            Users.Clear();
            Users.AddRange(Mapper.Map<User[]>(_employeeUnitOfWork.UserRepository.Get()));
        }

        private void LoadUserGroups()
        {
            UserGroups.Clear();
            UserGroups.AddRange(Mapper.Map<UserGroup[]>(_employeeUnitOfWork.UserGroupRepository.Get()));
        }
    }
}