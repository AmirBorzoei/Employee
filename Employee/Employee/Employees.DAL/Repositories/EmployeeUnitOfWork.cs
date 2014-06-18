using System;
using Employees.DAL.Entities;

namespace Employees.DAL.Repositories
{
    public interface IEmployeeUnitOfWork
    {
        UserGroupRepository UserGroupRepository { get; }
        UserRepository UserRepository { get; }
        GenericRepository<EmployeeEntity> EmployeeRepository { get; }

        void SaveChanges();
    }

    public class EmployeeUnitOfWork : IEmployeeUnitOfWork, IDisposable
    {
        private readonly EmployeeContext _context;
        private readonly GenericRepository<EmployeeEntity> _employeeRepository;
        private readonly UserGroupRepository _userGroupRepository;
        private readonly UserRepository _userRepository;
        private bool _disposed;


        public EmployeeUnitOfWork(EmployeeContext context)
        {
            _context = context;

            //_employeeRepository = new GenericRepository<EmployeeEntity>();
            _userGroupRepository = new UserGroupRepository(_context);
            _userRepository = new UserRepository(_context, _userGroupRepository);
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

        public void SaveChanges()
        {
            _context.SaveChanges();
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