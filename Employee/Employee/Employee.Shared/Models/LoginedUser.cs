using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Employees.Shared.Constants;

namespace Employees.Shared.Models
{
    public class LoginedUser : BaseModel
    {
        public LoginedUser()
        {
            Permissions = new BindableCollection<UserPermission>();
        }


        public User User { get; set; }
        public BindableCollection<UserPermission> Permissions { get; private set; }


        public bool HasAccess(string permissionKeyFullName, PermissionAccessTypes permissionAccessType)
        {
            var userPermission = Permissions.FirstOrDefault(p => p.PermissionKeyFullName == permissionKeyFullName);
            return userPermission != null && userPermission.PermissionAccessType == permissionAccessType;
        }
    }
}