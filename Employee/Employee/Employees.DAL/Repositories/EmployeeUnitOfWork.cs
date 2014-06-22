using Employees.DAL.Entities;

namespace Employees.DAL.Repositories
{
    public interface IEmployeeUnitOfWork
    {
        UserGroupRepository UserGroupRepository { get; }
        UserRepository UserRepository { get; }
        PermissionKeyRepository PermissionKeyRepository { get; }

        GenericRepository<EmployeeEntity> EmployeeRepository { get; }
    }

    public class EmployeeUnitOfWork : IEmployeeUnitOfWork
    {
        private readonly GenericRepository<EmployeeEntity> _employeeRepository;
        private readonly UserGroupRepository _userGroupRepository;
        private readonly UserRepository _userRepository;
        private readonly PermissionKeyRepository _permissionKeyRepository;


        public EmployeeUnitOfWork(GenericRepository<EmployeeEntity> employeeRepository,
            UserGroupRepository userGroupRepository,
            UserRepository userRepository,
            PermissionKeyRepository permissionKeyRepository)
        {
            _employeeRepository = employeeRepository;
            _userGroupRepository = userGroupRepository;
            _userRepository = userRepository;
            _permissionKeyRepository = permissionKeyRepository;
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

        public PermissionKeyRepository PermissionKeyRepository
        {
            get { return _permissionKeyRepository; }
        }
    }
}