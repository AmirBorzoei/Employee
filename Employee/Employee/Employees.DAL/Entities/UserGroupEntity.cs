using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employees.DAL.Entities
{
    [Table("UserGroup")]
    public class UserGroupEntity
    {
        [Key]
        public long UserGroupId { get; set; }

        public string UserGroupName { get; set; }
        public List<UserEntity> Users { get; set; }
    }
}