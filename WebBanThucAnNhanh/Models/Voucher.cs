using System;
using System.ComponentModel.DataAnnotations;

namespace WebBanThucAnNhanh.Models
{
    public class Voucher
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        // 1: Phẩn trăm (%), 2: Số tiền cố định
        [Required]
        public int DiscountType { get; set; }

        // Giá trị giảm (Ví dụ: 20 cho 20%, hoặc 50000 cho 50.000đ)
        [Required]
        public decimal DiscountValue { get; set; }

        // Số tiền giảm tối đa (Áp dụng cho loại phần trăm)
        public decimal MaxDiscountAmount { get; set; }

        // Giá trị đơn hàng tối thiểu để áp dụng
        public decimal MinOrderValue { get; set; }

        public DateTime ExpiryDate { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
