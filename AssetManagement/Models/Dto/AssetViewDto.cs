using System.ComponentModel;

namespace AssetManagement.Models.Dto
{
    public class AssetViewDto
    {
        [DisplayName("Item No.")]
        public int AssetId { get; set; }

        [DisplayName("Date of Last Order")]
        public DateTime LastOrderDate { get; set; }

        //public string Model { get; set; }

        [DisplayName("Item Name")]
        public string ItemName { get; set; }

        [DisplayName("Vendor")]
        public string Vendor { get; set; }

        [DisplayName("Vendor Contact No.")]
        public string VendorContact { get; set; }

        [DisplayName("Vendor Email ID, URL")]
        public string VendorEmail { get; set; }

        [DisplayName("Stock Location")]
        public string StockLocation { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Cost Per Item")]
        public long CostPerItem { get; set; }

        [DisplayName("Stock Quantity")]
        public long StockQuantity { get; set; }

        [DisplayName("Total Value")]
        public long TotalValue { get; set; }

        [DisplayName("Reorder Level")]
        public int ReorderLevel { get; set; }

        [DisplayName("Days Per Reorder")]
        public int DaysPerReorder { get; set; }

        [DisplayName("Item Reorder Quantity")]
        public int ItemReorderQuantity { get; set; }

        [DisplayName("Item Discontinued")]
        public bool ItemDiscontinued { get; set; }

        [DisplayName("Item Discontinued")]
        public string ItemDiscontinuedStatus { get
            {
                return ItemDiscontinued == true ? "Yes" : "No";
            }
        }
    }
}
