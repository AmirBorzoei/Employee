using Employees.DAL.Entities;

namespace Employees.DAL.Repositories
{
    public class UserGroupPermissionRepository : GenericRepository<UserGroupPermissionEntity>
    {
        public void DeleteUserGroupPermissionById(long id)
        {
            using (var context = GetDbContext())
            {
                Delete(context, id);
                context.SaveChanges();
            }
        }
    }
}