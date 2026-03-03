using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebBanThucAnNhanh.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Tên đăng nhập từ 3-50 ký tự")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        [StringLength(100)]
        public string FullName { get; set; }

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public string Role { get; set; } = "Customer"; 
        public bool Status { get; set; } = true;

        public int DrinkSpins { get; set; } = 0;
        public int FoodSpins { get; set; } = 0;

        // --- BỔ SUNG PHẦN TÍCH ĐIỂM & HẠNG THÀNH VIÊN ---
        [Display(Name = "Điểm tích lũy")]
        public int LoyaltyPoints { get; set; } = 0;

        [Display(Name = "Tổng chi tiêu")]
        public decimal TotalSpent { get; set; } = 0m;

        [Display(Name = "Hạng thành viên")]
        public string MembershipLevel { get; set; } = "Đồng"; // Mặc định là Đồng

        public virtual ICollection<Order>? Orders { get; set; }
        public virtual ICollection<UserReward>? UserRewards { get; set; }
    }
}