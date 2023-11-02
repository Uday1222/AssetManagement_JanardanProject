using System.ComponentModel;

namespace AssetManagement.Models.Dto
{
    public class VendorViewDto
    {
        [DisplayName("Vendor Id")]
        public int VendorId { get; set; }

        [DisplayName("Vendor Name")]
        public string VendorName { get; set; }

        [DisplayName("Product Name")]
        public string productName { get; set; }

        [DisplayName("Web Link")]
        public string WebLink { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Lead Time in Days")]
        public int LeadTimeInDays { get; set; }

        [DisplayName("Contact Name")]
        public string ContactName { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Phone")]
        public string Phone { get; set; }

        [DisplayName("Fax")]
        public string Fax { get; set; }

        [DisplayName("Mailing Address")]
        public string MailingAddress { get; set; }

        [DisplayName("City")]
        public string City { get; set; }

        [DisplayName("State")]
        public string State { get; set; }

        [DisplayName("Pin")]
        public string Pin { get; set; }
    }
}
