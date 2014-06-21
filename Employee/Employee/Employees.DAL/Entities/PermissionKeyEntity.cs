using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employees.DAL.Entities
{
    [Table("PermissionKey")]
    public class PermissionKeyEntity
    {
        [Key]
        public long PermissionKeyId { get; set; }

        public long TreeId { get; set; }
        public long TreeParentId { get; set; }
        public string PermissionKeyName { get; set; }
        public string PermissionKeyLabel { get; set; }
    }
}