using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanThucAnNhanh.Models
{
    public class CrossSellBundle
{
    public int Id { get; set; }
    
    // Món chính
    public int MainFastFoodId { get; set; }
    // THÊM VIRTUAL VÀO ĐÂY
    public virtual FastFood MainFastFood { get; set; }
    
    // Món mua kèm
    public int AddOnFastFoodId { get; set; }
    // THÊM VIRTUAL VÀO ĐÂY
    public virtual FastFood AddOnFastFood { get; set; }
    
    // Phần trăm giảm giá
    public double DiscountPercentage { get; set; } 
}
}