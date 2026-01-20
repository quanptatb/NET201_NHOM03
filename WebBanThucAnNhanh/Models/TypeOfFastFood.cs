using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // Thêm nếu cần dùng InverseProperty

namespace WebBanThucAnNhanh.Models
{
    public class TypeOfFastFood
    {
        [Key]
        public int IdTypeOfFastFood { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên loại")]
        [StringLength(100, ErrorMessage = "Tên loại không được quá 100 ký tự")]
        [Display(Name = "Tên loại")]
        public string NameTypeOfFastFood { get; set; }

        // Thêm dòng này để tạo quan hệ 1-nhiều (1 loại có nhiều món)
        // Giúp bạn dễ dàng truy vấn: var listMonAn = typeObj.FastFoods;
        public ICollection<FastFood>? FastFoods { get; set; }
    }
}