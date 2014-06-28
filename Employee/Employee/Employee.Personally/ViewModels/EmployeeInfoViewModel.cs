using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using Caliburn.Micro;
using Employees.DAL;
using Employees.DAL.Entities;
using Employees.DAL.Repositories;
using Employees.Shared.Interfaces;
using Employees.Shared.Models;
using Employees.Shared.Results;

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
            ProgressBarResult.Show().ExecuteAsync();

            var t = new Task(() =>
            {
                CurrentEmployee = _employeeUnitOfWork.EmployeeRepository.UpdateOrInsert(CurrentEmployee);

                RefreshAllEmployees();

                ProgressBarResult.Hide().ExecuteAsync();
            });
            t.Start();
        }

        public void Reload()
        {
            ProgressBarResult.Show().ExecuteAsync();

            var t = new Task(() =>
            {
                RefreshAllEmployees();
                ProgressBarResult.Hide().ExecuteAsync();
            });
            t.Start();
        }

        public void Print()
        {
        }


        private void RefreshAllEmployees()
        {
            AllEmployees.Clear();

            AllEmployees.AddRange(_employeeUnitOfWork.EmployeeRepository.GetUserGroups());
        }
    }
}