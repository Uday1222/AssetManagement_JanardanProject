using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AssetManagement.Models.Dto
{
    public class AssetDetailsViewDto
    {
        public AssetDetailsViewDto()
        {
            Status = new List<SelectListItem>
            {
                new SelectListItem {Text = "Received", Value = "Received"},
                new SelectListItem {Text = "Return", Value = "Return"},
            };
        }
        public string SerialNo { get; set; }

        public string EmpId { get; set; }

        public string EmpName { get; set; }

        public DateTime GivenDate { get; set; }

        public string SelectedStatus { get; set; }

        public List<SelectListItem> Status { get; set; }

        public string? AdditionalDetails { get; set; }

        public string? Comments { get; set; }
    }
}
