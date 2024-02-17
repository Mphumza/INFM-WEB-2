using System.ComponentModel.DataAnnotations.Schema;

namespace INFM_WEB_2.Models
{
    [Table("CartDetail")]
    public class CartDetail
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Product_Id { get; set; }
        public Product Products { get; set; }
        [Required]
        public int ShoppingCart_Id { get; set; }
        public ShoppingCart ShoppingCart { get; set; }

        public int Quantity { get; set; }
        [Required]
        public decimal UnitPrice {  get; set; }
    }
}
