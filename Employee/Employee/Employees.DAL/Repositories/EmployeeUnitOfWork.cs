using System;
using Employees.DAL.Entities;

namespace Employees.DAL.Repositories
{
    public interface IEmployeeUnitOfWork
    {
        GenericRepository<UserGroupEntity> UserGroupRepository { get; }
        GenericRepository<UserEntity> UserRepository { get; }
        GenericRepository<EmployeeEntity> EmployeeRepository { get; }
    }

    public class EmployeeUnitOfWork : IEmployeeUnitOfWork, IDisposable
    {
        private readonly EmployeeContext _context;
        private readonly GenericRepository<EmployeeEntity> _employeeRepository;
        private readonly GenericRepository<UserGroupEntity> _userGroupRepository;
        private readonly GenericRepository<UserEntity> _userRepository;
        private bool _disposed;


        public EmployeeUnitOfWork(EmployeeContext context)
        {
            _context = context;

            _employeeRepository = new GenericRepository<EmployeeEntity>();
            _userGroupRepository = new GenericRepository<UserGroupEntity>();
            _userRepository = new GenericRepository<UserEntity>();
        }


        public GenericRepository<EmployeeEntity> EmployeeRepository
        {
            get { return _employeeRepository; }
        }

        public GenericRepository<UserGroupEntity> UserGroupRepository
        {
            get { return _userGroupRepository; }
        }

        public GenericRepository<UserEntity> UserRepository
        {
            get { return _userRepository; }
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }
    }
}