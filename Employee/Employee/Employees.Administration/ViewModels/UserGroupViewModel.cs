﻿using AutoMapper;
using Employees.DAL;
using Employees.DAL.Entities;
using Employees.DAL.Repositories;
using Employees.Shared.Constants;
using Employees.Shared.Interfaces;
using Employees.Shared.Models;
using Employees.Shared.ViewModels;

namespace Employees.Administration.ViewModels
{
    public interface IUserGroupViewModel : IScreenBase, ISupportNew, ISupportSave, ISupportReload
    {
        UserGroup CurrentObject { get; set; }
    }

    public class UserGroupViewModel : ScreenBase<UserGroup>, IUserGroupViewModel
    {
        private readonly IEmployeeUnitOfWork _employeeUnitOfWork;


        public UserGroupViewModel(IEmployeeUnitOfWork employeeUnitOfWork)
        {
            _employeeUnitOfWork = employeeUnitOfWork;
        }


        public void New()
        {
            CurrentObject = new UserGroup();
        }

        public void Save()
        {
            if (CurrentObject == null) return;

            //var userGroupEntity = _employeeUnitOfWork.UserGroupRepository.GetByID(CurrentObject.UserGroupId);

            var userGroupEntity = Mapper.Map<UserGroupEntity>(CurrentObject);

            if (CurrentObject.State == ModelStates.New)
                CurrentObject = Mapper.Map<UserGroup>(_employeeUnitOfWork.UserGroupRepository.Insert(userGroupEntity));
            if (CurrentObject.State == ModelStates.Modified)
                CurrentObject = Mapper.Map<UserGroup>(_employeeUnitOfWork.UserGroupRepository.Update(userGroupEntity));
        }

        public void Reload()
        {
            if (CurrentObject == null || CurrentObject.State == ModelStates.New) return;

            CurrentObject = Mapper.Map<UserGroup>(_employeeUnitOfWork.UserGroupRepository.GetByID(CurrentObject.UserGroupId));
        }
    }
}