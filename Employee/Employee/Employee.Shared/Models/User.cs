using System.Collections.Generic;
using Caliburn.Micro;

namespace Employees.Shared.Models
{
    public class User : BaseModel
    {
        private long _userId;
        private string _userName;
        private string _password;
        private string _firstName;
        private string _lastName;
        private BindableCollection<UserGroup> _userGroups;


        public User()
        {
            _userGroups = new BindableCollection<UserGroup>();
        }


        public long UserId
        {
            get { return _userId; }
            set
            {
                _userId = value;
                NotifyOfPropertyChange(() => UserId);
            }
        }

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                NotifyOfPropertyChange(() => UserName);
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
            }
        }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                NotifyOfPropertyChange(() => FirstName);
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                NotifyOfPropertyChange(() => LastName);
            }
        }

        public BindableCollection<UserGroup> UserGroups
        {
            get { return _userGroups; }
            set
            {
                _userGroups = value;
                NotifyOfPropertyChange(() => UserGroups);
            }
        }
    }
}