
namespace INFM_WEB_2.Models
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public int Product_Id { get; set; }

        [Required]
        public string? Product_Name { get; set; }

        [Required]
        [DisplayName("Product Price")]
        public decimal Product_Price { get; set; }

        [Required]
        [DisplayName("Product Description")]
        public string? ProductDescription { get; set; }

        [Required]
        [DisplayName("Stock Count")]
        public int ProductCount { get; set; }

        public int Category_Id { get; set; }
        public Category? Category { get; set; }

        public int Supplier_Id { get; set; }
        public Supplier? Supplier { get; set; }

        public List<CartDetail>? CartDetail { get; set; }
        public List<OrderDetail>? OrderDetail { get; set; }
        [NotMapped]
        public string? Category_Name { get; set; }
    }
}
