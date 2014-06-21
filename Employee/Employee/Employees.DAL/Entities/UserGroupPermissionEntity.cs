using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Employees.Shared.Constants;

namespace Employees.DAL.Entities
{
    [Table("UserGroupPermission")]
    public class UserGroupPermissionEntity
    {
        [Key]
        public long UserGroupPermissionId { get; set; }

        public UserGroupEntity UserGroupEntity { get; set; }
        public PermissionKeyEntity PermissionKeyEntity { get; set; }
        public PermissionAccessTypes PermissionAccessType { get; set; }
    }
}