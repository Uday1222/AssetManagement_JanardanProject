using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetManagement.Models
{
    public class AssetDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AssetDetailsId { get; set; }

        public string SerialNo { get; set; }

        public string EmpId { get; set; }

        public string EmpName { get; set; }

        public DateTime GivenDate { get; set; }

        public string Status { get; set; }

        public string? AdditionalDetails { get; set; }

        public string? Comments { get; set; }

    }
}
