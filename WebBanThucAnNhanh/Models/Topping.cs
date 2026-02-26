using System.ComponentModel.DataAnnotations;

namespace WebBanThucAnNhanh.Models
{
    public class Topping
    {
        [Key]
        public int IdTopping { get; set; }
        
        [Required]
        [Display(Name = "Tên tùy chọn (Size/Topping)")]
        public string Name { get; set; }
        
        [Required]
        [Display(Name = "Giá cộng thêm")]
        public decimal Price { get; set; }
    }
}