using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanThucAnNhanh.Models
{
    public class UserReward
    {
        [Key]
        public int Id { get; set; }

        // Khóa ngoại tới User
        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        // Khóa ngoại tới WheelPrize
        [Required]
        public int PrizeId { get; set; }

        [ForeignKey("PrizeId")]
        public virtual WheelPrize WheelPrize { get; set; }

        // Đã sử dụng chưa (mặc định false)
        [Display(Name = "Đã sử dụng")]
        public bool IsUsed { get; set; } = false;

        // Ngày trúng thưởng
        [Display(Name = "Ngày trúng")]
        public DateTime DateWon { get; set; } = DateTime.Now;
    }
}
