using AutoMapper;
using Employees.DAL.Entities;
using Employees.DAL.Criteria;
using Employees.Shared.Constants;
using Employees.Shared.Models;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace Employees.DAL.Repositories
{
    public class EmployeeRepository : GenericRepository<EmployeeEntity>
    {
        public List<Employee> GetUserGroups(SearchQuery<EmployeeEntity> searchQuery = null)
        {
            using (var context = GetDbContext())
            {
                if (searchQuery == null)
                    searchQuery = new SearchQuery<EmployeeEntity>();
                if (searchQuery.SortCriterias.Count == 0)
                {
                    searchQuery.AddSortCriteria(new ExpressionSortCriteria<EmployeeEntity, string>(ug => ug.PersonallyCode, SortDirection.Ascending));
                }

                var employeeEntities = base.Get(context, searchQuery);
                return Mapper.Map<List<EmployeeEntity>, List<Employee>>(employeeEntities);
            }
        }

        public Employee UpdateOrInsert(Employee employee)
        {
            using (var context = GetDbContext())
            {
                EmployeeEntity returnEntity = null;

                if (employee.State == ModelStates.New)
                {
                    var employeeEntity = Mapper.Map<Employee, EmployeeEntity>(employee);

                    returnEntity = Insert(context, employeeEntity);

                    context.SaveChanges();
                }
                else if (employee.IsDirty)
                {
                    var employeeEntity = GetByID(context, employee.EmployeeId);

                    Mapper.Map(employee, employeeEntity);

                    Update(context, employeeEntity);

                    context.SaveChanges();

                    returnEntity = GetByID(context, employeeEntity.EmployeeId);
                }

                return Mapper.Map<Employee>(returnEntity);
            }
        }
    }
}