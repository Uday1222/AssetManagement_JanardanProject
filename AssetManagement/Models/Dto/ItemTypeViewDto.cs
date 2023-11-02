using System.ComponentModel;

namespace AssetManagement.Models.Dto
{
    public class ItemTypeViewDto
    {
        public int ItemTypeId { get; set; }

        [DisplayName("Item Type")]
        public string ItemType { get; set; }
    }
}
