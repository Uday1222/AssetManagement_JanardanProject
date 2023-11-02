using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetManagement.Models
{
    public class Vendor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VendorId { get; set; }

        public string VendorName { get; set; }

        public string productName { get; set; }

        public string WebLink { get; set; }

        public string Description { get; set; }

        public int LeadTimeInDays { get; set; }

        public string ContactName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public string MailingAddress { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Pin { get; set; }
    }
}
