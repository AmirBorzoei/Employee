using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using AutoMapper;
using Caliburn.Micro;
using Employees.DAL;
using Employees.DAL.Entities;
using Employees.DAL.Repositories;
using Employees.Shared.Interfaces;
using Employees.Shared.Models;

namespace Employees.Personally.ViewModels
{
    public interface IEmployeeInfoViewModel : IScreen, ISupportSave, ISupportPrint
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

            //FillAllEmployees();
        }


        public void Save()
        {
            var employeeEntity = Mapper.Map<Employee, EmployeeEntity>(CurrentEmployee);
            _employeeUnitOfWork.EmployeeRepository.Insert(employeeEntity);
        }

        public void Print()
        {
        }

        public void SayHello()
        {
            //_windowManager.ShowWindow(_dialogViewModel);
        }


        private void FillAllEmployees()
        {
            AllEmployees.AddRange(Mapper.Map<IEnumerable<EmployeeEntity>, IEnumerable<Employee>>(_employeeUnitOfWork.EmployeeRepository.Get()));
        }

        private void AddNewEmployeeToAllEmployees(Employee newEmployee)
        {
            AllEmployees.Add(newEmployee);
        }

        private void RefreshAllEmployees()
        {
            //AllEmployees.Clear();

            //using (var db = new EmployeeContext())
            //{
            //    AllEmployees.AddRange(db.Employees.ToList());
            //}
        }
    }
}