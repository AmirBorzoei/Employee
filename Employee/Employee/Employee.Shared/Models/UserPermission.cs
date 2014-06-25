using Employees.Shared.Constants;

namespace Employees.Shared.Models
{
    public class UserPermission
    {
        public string PermissionKeyFullName { get; set; }
        public PermissionAccessTypes PermissionAccessType { get; set; }
    }
}