namespace INFM_WEB_2.Models
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public int Order_Id { get; set; }
        [Required]
        public string? UserId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [Required]
        public int OrderStatusId { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public List<OrderDetail> Details { get; set; }
    }
}
