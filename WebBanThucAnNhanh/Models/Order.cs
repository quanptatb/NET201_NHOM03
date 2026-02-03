using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanThucAnNhanh.Models
{
    public class Order
    {
        [Key]
        public int IdOrder { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;

        [Required]
        public decimal TotalPrice { get; set; }

        // Trạng thái: 0-Chưa giao, 1-Đang giao, 2-Đã giao, 3-Hủy
        public int Status { get; set; } = 0;

        public string ShippingAddress { get; set; }
        public string PhoneNumber { get; set; }

        // Khóa ngoại liên kết với User
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        // Chi tiết đơn hàng
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}