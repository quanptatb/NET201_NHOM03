using System.ComponentModel.DataAnnotations;

namespace WebBanThucAnNhanh.Models
{
    public class Combo
    {
        [Key]
        public int IdCombo { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên Combo")]
        [StringLength(100)]
        public string NameCombo { get; set; }

        [Required]
        [Range(0, 999999999, ErrorMessage = "Giá phải lớn hơn 0")]
        public decimal Price { get; set; } // SỬA: double -> decimal

        public string Image { get; set; }

        public string Description { get; set; }

        // Món ăn trong Combo (tùy độ phức tạp, ở mức cơ bản có thể chỉ cần mô tả text, 
        // nhưng tốt nhất là danh sách món nếu muốn trừ kho).
        // Ở đây làm mức đơn giản theo Assignment C#4:
        public string ComboItemsList { get; set; } // Ví dụ: "1 Gà + 1 Pepsi"
    }
}