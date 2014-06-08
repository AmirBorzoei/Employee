using System;
using Employees.Shared.Constants;

namespace Employees.Shared.Models
{
    public class UserGroup : BaseModel, IEquatable<UserGroup>
    {
        private long _userGroupId;
        private string _userGroupName;

        public long UserGroupId
        {
            get { return _userGroupId; }
            set
            {
                _userGroupId = value;
                NotifyOfPropertyChange(() => UserGroupId);
            }
        }

        public string UserGroupName
        {
            get { return _userGroupName; }
            set
            {
                _userGroupName = value;
                NotifyOfPropertyChange(() => UserGroupName);
            }
        }

        public override string DisplayName
        {
            get { return UserGroupName; }
        }


        public UserGroup GetCopy(bool getNew)
        {
            var copy = new UserGroup
            {
                UserGroupId = UserGroupId,
                UserGroupName = UserGroupName,
                State = getNew ? ModelStates.New : State,
            };

            return copy;
        }


        public bool Equals(UserGroup other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _userGroupId == other._userGroupId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((UserGroup) obj);
        }

        public override int GetHashCode()
        {
            return _userGroupId.GetHashCode();
        }
    }
}