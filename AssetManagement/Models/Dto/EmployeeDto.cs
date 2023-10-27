using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AssetManagement.Models.Dto
{
    public class EmployeeDto
    {
        [Required]
        public string EmpName { get; set; }
    }
}
