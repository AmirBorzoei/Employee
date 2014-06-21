using System.Data.Entity;
using Employees.DAL.Entities;

namespace Employees.DAL
{
    public class EmployeeContext : DbContext
    {
        public DbSet<EmployeeEntity> Employees { get; set; }

        public DbSet<UserGroupEntity> UserGroups { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<PermissionKeyEntity> PermissionKeis { get; set; }


        static EmployeeContext()
        {
            Database.SetInitializer<EmployeeContext>(new EmployeeContextInitializer());
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>()
                .HasMany(t => t.UserGroups)
                .WithMany(t => t.Users)
                .Map(m =>
                {
                    m.ToTable("UserGroupUser");
                    m.MapLeftKey("UserId");
                    m.MapRightKey("UserGroupId");
                });
        }
    }
}