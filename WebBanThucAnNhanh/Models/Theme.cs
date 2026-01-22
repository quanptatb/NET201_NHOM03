using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // Thêm nếu cần dùng InverseProperty

namespace WebBanThucAnNhanh.Models
{
    public class Theme
    {
        [Key]
        public int IdTheme { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên chủ đề")]
        [StringLength(100, ErrorMessage = "Tên chủ đề không được quá 100 ký tự")]
        [Display(Name = "Tên chủ đề")]
        public string NameTheme { get; set; }

        // Thêm dòng này để tạo quan hệ 1-nhiều (1 loại có nhiều món)
        // Giúp bạn dễ dàng truy vấn: var listMonAn = typeObj.FastFoods;
        public ICollection<FastFood>? FastFoods { get; set; }
    }
}