using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetManagement.Models
{
    public class Asset
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AssetId { get; set; }

        public string Model { get; set; }

        public string ItemType { get; set; }

        public string SerialNo { get; set; }

        public string PurchasedBy { get; set;  }

        public string? Status { get; set; }

    }
}
