
namespace INFM_WEB_2.Models
{
    public class Supplier
    {
        [Key]
        public int Supplier_Id { get; set; }
        [Required]
        public string SupplierName { get; set; }
        [Required]
        [Display(Name = "Street Address")]
        public string Address { get; set; }
        [Required]
        [Display(Name = "City")]
        public string City { get; set; }
        [Required]
        [Display(Name = "Province")]
        public string Province { get; set; }
        [Required]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }
        [Required]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Phone")]
        public string Phone { get; set; }
        [Required]
        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }
    }
}
