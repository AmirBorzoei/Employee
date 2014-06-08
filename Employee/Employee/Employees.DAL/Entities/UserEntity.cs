using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employees.DAL.Entities
{
    [Table("User")]
    public class UserEntity
    {
        [Key]
        public long UserId { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<UserGroupEntity> UserGroups { get; set; }
    }
}