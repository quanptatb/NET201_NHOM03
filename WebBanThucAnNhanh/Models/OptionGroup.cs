using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebBanThucAnNhanh.Models
{
    public class OptionGroup
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; } // Ví dụ: "Size", "Topping", "Đá", "Đường"
        
        // Cờ xác định user có thể chọn nhiều (ví dụ Topping) hay chỉ 1 (ví dụ Size)
        public bool IsMultiSelect { get; set; } 

        public virtual ICollection<OptionItem> OptionItems { get; set; }
    }
}