using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanThucAnNhanh.Models
{
    // Bảng trung gian n-n giữa FastFood và OptionGroup
    // (Vì Burger thì chỉ có Size, Trà sữa thì có cả Size và Topping)
    public class FastFoodOptionGroup
    {
        public int FastFoodId { get; set; }
        [ForeignKey("FastFoodId")]
        public virtual FastFood FastFood { get; set; }

        public int OptionGroupId { get; set; }
        [ForeignKey("OptionGroupId")]
        public virtual OptionGroup OptionGroup { get; set; }
    }
}