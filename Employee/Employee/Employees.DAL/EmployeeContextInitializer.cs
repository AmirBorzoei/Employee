using System.Data.Entity;
using Employees.DAL.Entities;

namespace Employees.DAL
{
    public class EmployeeContextInitializer : DropCreateDatabaseAlways<EmployeeContext>
    {
        protected override void Seed(EmployeeContext context)
        {
            context.PermissionKeis.Add(new PermissionKeyEntity {TreeId = 100000, PermissionKeyName = "AdministrationModule", PermissionKeyLabel = "مدیریت"});
            context.PermissionKeis.Add(new PermissionKeyEntity {TreeId = 200000, PermissionKeyName = "BasicDataModule", PermissionKeyLabel = "اطلاعات پایه"});
            context.PermissionKeis.Add(new PermissionKeyEntity {TreeId = 300000, PermissionKeyName = "FinancialModule", PermissionKeyLabel = "حسابداری"});
            context.PermissionKeis.Add(new PermissionKeyEntity {TreeId = 400000, PermissionKeyName = "PersonallyModule", PermissionKeyLabel = "پرسنلی"});
        }
    }
}