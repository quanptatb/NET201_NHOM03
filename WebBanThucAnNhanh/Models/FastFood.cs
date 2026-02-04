using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Range(0, 99999999999, ErrorMessage = "Giá phải lớn hơn 0")]
        [Display(Name = "Giá bán")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng")]
        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }

        // QUAN TRỌNG: Thêm dấu ? để không bị lỗi Required khi chưa kịp gán tên file
        [Display(Name = "Hình ảnh")]
        public string? Image { get; set; }

        [Display(Name = "Trạng thái (Còn hàng)")]
        public bool Status { get; set; }

        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        // Khóa ngoại
        [Display(Name = "Loại thức ăn")]
        public int IdTypeOfFastFood { get; set; }

        [ForeignKey("IdTypeOfFastFood")]
        public virtual TypeOfFastFood? TypeOfFastFood { get; set; }

        // Khóa ngoại
        [Display(Name = "Chủ đề")]
        public int IdTheme { get; set; }

        [ForeignKey("IdTheme")]
        public virtual Theme? Theme { get; set; }
    }
}