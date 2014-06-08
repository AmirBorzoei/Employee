using Caliburn.Micro;

namespace Employees.Shared.Models
{
    public class Employee : BaseModel
    {
        private long _employeeId;
        private string _personallyCode;
        private string _nationalCode;
        private string _firstName;
        private string _lastName;
        private string _fatherName;
        private int _familyCount;
        private int _age;
        private int _workHistory;
        private bool _isMarried;


        public long EmployeeId
        {
            get { return _employeeId; }
            set
            {
                _employeeId = value;
                NotifyOfPropertyChange(() => EmployeeId);
            }
        }

        public string PersonallyCode
        {
            get { return _personallyCode; }
            set
            {
                _personallyCode = value;
                NotifyOfPropertyChange(() => PersonallyCode);
            }
        }

        public string NationalCode
        {
            get { return _nationalCode; }
            set
            {
                _nationalCode = value;
                NotifyOfPropertyChange(() => NationalCode);
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

        public string FatherName
        {
            get { return _fatherName; }
            set
            {
                _fatherName = value;
                NotifyOfPropertyChange(() => FatherName);
            }
        }

        public bool IsMarried
        {
            get { return _isMarried; }
            set
            {
                _isMarried = value;
                NotifyOfPropertyChange(() => IsMarried);
            }
        }

        public int FamilyCount
        {
            get { return _familyCount; }
            set
            {
                _familyCount = value;
                NotifyOfPropertyChange(() => FamilyCount);
            }
        }

        public int Age
        {
            get { return _age; }
            set
            {
                _age = value;
                NotifyOfPropertyChange(() => Age);
            }
        }

        public int WorkHistory
        {
            get { return _workHistory; }
            set
            {
                _workHistory = value;
                NotifyOfPropertyChange(() => WorkHistory);
            }
        }
    }
}