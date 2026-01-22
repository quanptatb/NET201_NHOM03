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
        [Range(0, 99999999999, ErrorMessage = "Giá phải lớn hơn 0")] // Sửa Range lại một chút cho an toàn
        [Display(Name = "Giá bán")]
        public decimal Price { get; set; } // Đổi double thành decimal

        [Required(ErrorMessage = "Vui lòng nhập số lượng")]
        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }

        [Display(Name = "Hình ảnh")]
        public string Image { get; set; }

        [Display(Name = "Trạng thái (Còn hàng)")]
        public bool Status { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        // ... code cũ ...

        // Khóa ngoại
        [Display(Name = "Loại thức ăn")]
        public int IdTypeOfFastFood { get; set; }

        [ForeignKey("IdTypeOfFastFood")]
        // THÊM DẤU ? VÀO SAU TypeOfFastFood
        public virtual TypeOfFastFood? TypeOfFastFood { get; set; }

        // Khóa ngoại
        [Display(Name = "Chủ đề")]
        public int IdTheme { get; set; }

        [ForeignKey("IdTheme")]
        // THÊM DẤU ? VÀO SAU Theme
        public virtual Theme? Theme { get; set; }

    }
}