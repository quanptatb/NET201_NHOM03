using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // Thêm namespace này để dùng [ForeignKey] nếu cần rõ ràng hơn
using System.Linq;
using System.Threading.Tasks;

namespace WebBanThucAnNhanh.Models
{
    public class FastFood
    {
        [Key]
        public int IdFastFood { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên món ăn")]
        [StringLength(100, ErrorMessage = "Tên món không được vượt quá 100 ký tự")]
        [Display(Name = "Tên món ăn")]
        public string NameFastFood { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        [Display(Name = "Giá bán")]
        public double Price { get; set; } // Nếu đổi sang decimal thì sửa thành: public decimal Price { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng")]
        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }

        [Display(Name = "Hình ảnh")]
        public string Image { get; set; }

        [Display(Name = "Trạng thái (Còn hàng)")]
        public bool Status { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        // Khóa ngoại
        [Display(Name = "Loại thức ăn")]
        public int IdTypeOfFastFood { get; set; }

        [ForeignKey("IdTypeOfFastFood")] // Khai báo rõ ràng khóa ngoại (Optional - vì EF tự hiểu nhưng viết ra sẽ dễ đọc code hơn)
        public TypeOfFastFood TypeOfFastFood { get; set; }
    }
}