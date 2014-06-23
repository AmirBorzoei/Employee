namespace Employees.Shared.Models
{
    public class PermissionKey : BaseModel
    {
        private long _permissionKeyId;
        private long _treeId;
        private long _treeParentId;
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

        public long TreeId
        {
            get { return _treeId; }
            set
            {
                _treeId = value;
                NotifyOfPropertyChange(() => TreeId);
            }
        }

        public long TreeParentId
        {
            get { return _treeParentId; }
            set
            {
                _treeParentId = value;
                NotifyOfPropertyChange(() => TreeParentId);
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


        public override string DisplayName
        {
            get { return PermissionKeyName; }
        }
    }
}