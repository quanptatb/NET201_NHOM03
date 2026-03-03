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

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng")]
        [StringLength(200, ErrorMessage = "Địa chỉ không được vượt quá 200 ký tự")]
        public string ShippingAddress { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [RegularExpression(@"^0[0-9]{9}$", ErrorMessage = "Số điện thoại phải bắt đầu bằng số 0 và có đúng 10 chữ số")]
        public string PhoneNumber { get; set; }

        // Khóa ngoại liên kết với User
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        // Chi tiết đơn hàng
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        // Thêm hàm này vào bên trong class OrderController
        private string EvaluateMemberTier(int totalPoints)
        {
            if (totalPoints >= 5000) return "Diamond";
            if (totalPoints >= 2000) return "Gold";    
            if (totalPoints >= 500) return "Silver";   
            return "Bronze";                           
        }
    }
}

