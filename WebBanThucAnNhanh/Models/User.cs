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

        // Bổ sung để đủ 6 thông tin theo yêu cầu Assignment
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        [StringLength(100)]
        public string FullName { get; set; }

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public string Role { get; set; } = "Customer"; // Customer, Admin
        public bool Status { get; set; } = true;

        // === LUCKY WHEEL: Số lượt quay tích lũy ===
        [Display(Name = "Lượt quay Nước")]
        public int DrinkSpins { get; set; } = 0;

        [Display(Name = "Lượt quay Đồ ăn")]
        public int FoodSpins { get; set; } = 0;

        // === LOYALTY PROGRAM: Điểm và Hạng thành viên ===
        [Display(Name = "Điểm tích lũy")]
        public int LoyaltyPoints { get; set; } = 0;

        [Display(Name = "Hạng thành viên")]
        public string MemberRank { get; set; } = "Thành viên";

        // Quan hệ 1-n: Một User có nhiều đơn hàng
        public virtual ICollection<Order>? Orders { get; set; }

        // Quan hệ 1-n: Một User có nhiều phần thưởng đã trúng
        public virtual ICollection<UserReward>? UserRewards { get; set; }
    }
}