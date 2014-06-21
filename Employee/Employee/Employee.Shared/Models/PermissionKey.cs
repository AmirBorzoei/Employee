namespace Employees.Shared.Models
{
    public class PermissionKey : BaseModel
    {
        private long _permissionKeyId;
        private string _permissionKeyLabel;
        private string _permissionKeyName;


        public long PermissionKeyId
        {
            get { return _permissionKeyId; }
            set
            {
                _permissionKeyId = value;
                NotifyOfPropertyChange(() => PermissionKeyId);
            }
        }

        public string PermissionKeyName
        {
            get { return _permissionKeyName; }
            set
            {
                _permissionKeyName = value;
                NotifyOfPropertyChange(() => PermissionKeyName);
            }
        }

        public string PermissionKeyLabel
        {
            get { return _permissionKeyLabel; }
            set
            {
                _permissionKeyLabel = value;
                NotifyOfPropertyChange(() => PermissionKeyLabel);
            }
        }
    }
}