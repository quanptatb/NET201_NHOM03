using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanThucAnNhanh.Models
{
    public class WheelPrize
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên giải thưởng")]
        [StringLength(200)]
        [Display(Name = "Tên giải thưởng")]
        public string PrizeName { get; set; }

        // PrizeType: 1 = Nước, 2 = Đồ ăn
        [Required]
        [Display(Name = "Loại giải (1: Nước, 2: Đồ ăn)")]
        public int PrizeType { get; set; }

        // Khóa ngoại tới FastFood (nullable - có thể là giải "Chúc bạn may mắn lần sau")
        [Display(Name = "Món ăn/nước liên kết")]
        public int? FastFoodId { get; set; }

        [ForeignKey("FastFoodId")]
        public virtual FastFood? FastFood { get; set; }

        // Tỷ lệ trúng (%)
        [Required]
        [Range(0, 100, ErrorMessage = "Tỷ lệ phải từ 0 đến 100")]
        [Display(Name = "Tỷ lệ trúng (%)")]
        public double Probability { get; set; }

        // Số lượng giải còn lại
        [Required]
        [Range(0, int.MaxValue)]
        [Display(Name = "Số lượng còn lại")]
        public int RemainingQuantity { get; set; }

        // Bật/Tắt giải thưởng
        [Display(Name = "Kích hoạt")]
        public bool IsActive { get; set; } = true;
    }
}
