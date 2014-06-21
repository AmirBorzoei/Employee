using System;
using Employees.DAL.Entities;

namespace Employees.DAL.Repositories
{
    public interface IEmployeeUnitOfWork
    {
        UserGroupRepository UserGroupRepository { get; }
        UserRepository UserRepository { get; }
        GenericRepository<EmployeeEntity> EmployeeRepository { get; }
    }

    public class EmployeeUnitOfWork : IEmployeeUnitOfWork
    {
        private readonly GenericRepository<EmployeeEntity> _employeeRepository;
        private readonly UserGroupRepository _userGroupRepository;
        private readonly UserRepository _userRepository;
        private bool _disposed;


        public EmployeeUnitOfWork()
        {
            //_employeeRepository = new GenericRepository<EmployeeEntity>();
            _userGroupRepository = new UserGroupRepository();
            _userRepository = new UserRepository(_userGroupRepository);
        }


        public GenericRepository<EmployeeEntity> EmployeeRepository
        {
            get { return _employeeRepository; }
        }

        public UserGroupRepository UserGroupRepository
        {
            get { return _userGroupRepository; }
        }

        public UserRepository UserRepository
        {
            get { return _userRepository; }
        }
    }
}