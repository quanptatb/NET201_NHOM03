using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanThucAnNhanh.Models
{
    public class OrderDetail
    {
        [Key]
        public int IdOrderDetail { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; } // Giá tại thời điểm mua

        // Liên kết Order
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        // Liên kết FastFood (Có thể null nếu khách mua Combo)
        public int? FastFoodId { get; set; }
        [ForeignKey("FastFoodId")]
        public FastFood? FastFood { get; set; }

        // Liên kết Combo (Có thể null nếu khách mua Món lẻ)
        public int? ComboId { get; set; }
        [ForeignKey("ComboId")]
        public Combo? Combo { get; set; }
    }
}