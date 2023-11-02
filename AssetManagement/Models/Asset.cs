using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetManagement.Models
{
    public class Asset
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AssetId { get; set; }

        public DateTime LastOrderDate { get; set; }

        //public string Model { get; set; }

        public string ItemName { get; set; }

        public string Vendor { get; set; }

        public string VendorContact { get; set; }

        public string VendorEmail { get; set; }

        public string StockLocation { get; set; }

        public string Description { get; set; }

        public long CostPerItem { get; set; }

        public long TotalValue { get; set; }

        public int ReorderLevel { get; set; }

        public int DaysPerReorder { get; set; }

        public int ItemReorderQuantity {get; set;}

        public bool ItemDiscontinued { get; set; }

        public string? Status { get; set; }

    }
}
