using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employees.DAL.Entities
{
    [Table("Employee")]
    public class EmployeeEntity
    {
        [Key]
        public long EmployeeId { get; set; }

        public string PersonallyCode { get; set; }
        public string NationalCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public int FamilyCount { get; set; }
        public int Age { get; set; }
        public int WorkHistory { get; set; }
        public bool IsMarried { get; set; }
        public long CreateUserId { get; set; }
    }
}