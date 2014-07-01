using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using Employees.DAL.Repositories;
using Employees.Shared.Interfaces;
using Employees.Shared.Models;
using Employees.Shared.Services;

namespace Employees.Personally.ViewModels
{
    public interface IEmployeeInfoViewModel : IScreen, ISupportSave, ISupportPrint, ISupportNew, ISupportReload
    {
    }

    public class EmployeeInfoViewModel : Screen, IEmployeeInfoViewModel
    {
        private readonly IEmployeeUnitOfWork _employeeUnitOfWork;
        private readonly IWindowManager _windowManager;
        private Employee _currentEmployee;


        public EmployeeInfoViewModel(IWindowManager windowManager,
            IEmployeeUnitOfWork employeeUnitOfWork)
        {
            _windowManager = windowManager;
            _employeeUnitOfWork = employeeUnitOfWork;

            _currentEmployee = new Employee();
            AllEmployees = new BindableCollection<Employee>();

            DisplayName = "پرسنلی";
        }


        public Employee CurrentEmployee
        {
            get { return _currentEmployee; }
            set
            {
                _currentEmployee = value;
                NotifyOfPropertyChange(() => CurrentEmployee);
            }
        }

        public BindableCollection<Employee> AllEmployees { get; private set; }


        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            RefreshAllEmployees();
        }


        public void New()
        {
            CurrentEmployee = new Employee();
        }

        public void Save()
        {
            Task.Factory.StartNew(ProgressBarService.Show);

            var task = Task.Factory.StartNew(() =>
            {
                CurrentEmployee = _employeeUnitOfWork.EmployeeRepository.UpdateOrInsert(CurrentEmployee);

                RefreshAllEmployees();
            });

            task.ContinueWith(arg => Task.Factory.StartNew(ProgressBarService.Hide));
        }

        public void Reload()
        {
            Task.Factory.StartNew(ProgressBarService.Show);

            var task = Task.Factory.StartNew(RefreshAllEmployees);

            task.ContinueWith(arg => Task.Factory.StartNew(ProgressBarService.Hide));
        }

        public void Print(Window ownerWindow)
        {
            var view = GetView() as ISupportPrint;
            if (view != null)
                view.Print(ownerWindow);
        }


        private void RefreshAllEmployees()
        {
            AllEmployees.Clear();

            AllEmployees.AddRange(_employeeUnitOfWork.EmployeeRepository.GetUserGroups());
        }
    }
}