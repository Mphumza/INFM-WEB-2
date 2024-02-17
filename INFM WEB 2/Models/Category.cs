
namespace INFM_WEB_2.Models
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public int Category_Id { get; set; }
        [Required]
        [MaxLength(40)]
        public string CategoryName { get; set; }

        public List<Product> Products { get; set; }
    }
}
