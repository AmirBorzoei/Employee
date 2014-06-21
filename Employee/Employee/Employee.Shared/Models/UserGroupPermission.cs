using Employees.Shared.Constants;

namespace Employees.Shared.Models
{
    public class UserGroupPermission : BaseModel
    {
        private PermissionAccessTypes _permissionAccessType;
        private PermissionKey _permissionKey;
        private UserGroup _userGroup;
        private long _userGroupPermissionId;


        public long UserGroupPermissionId
        {
            get { return _userGroupPermissionId; }
            set
            {
                _userGroupPermissionId = value;
                NotifyOfPropertyChange(() => UserGroupPermissionId);
            }
        }

        public UserGroup UserGroup
        {
            get { return _userGroup; }
            set
            {
                _userGroup = value;
                NotifyOfPropertyChange(() => UserGroup);
            }
        }

        public PermissionKey PermissionKey
        {
            get { return _permissionKey; }
            set
            {
                _permissionKey = value;
                NotifyOfPropertyChange(() => PermissionKey);
            }
        }

        public PermissionAccessTypes PermissionAccessType
        {
            get { return _permissionAccessType; }
            set
            {
                _permissionAccessType = value;
                NotifyOfPropertyChange(() => PermissionAccessType);
            }
        }
    }
}