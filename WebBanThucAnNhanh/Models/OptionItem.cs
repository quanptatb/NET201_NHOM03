using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanThucAnNhanh.Models
{
    public class OptionItem
    {
        [Key]
        public int Id { get; set; }
        
        public int OptionGroupId { get; set; }
        [ForeignKey("OptionGroupId")]
        public virtual OptionGroup OptionGroup { get; set; }

        [Required]
        public string Name { get; set; } // Ví dụ: "Size L", "Trân châu trắng"
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal AdditionalPrice { get; set; } // Giá cộng thêm (VD: 10000)
    }
}