using System.Collections.Generic;

namespace WebBanThucAnNhanh.Models
{
    public class HomeViewModel
    {
        // Danh sách danh mục (để hiển thị bên trái)
        public List<TypeOfFastFood> ListCategories { get; set; }

        // Danh sách sản phẩm (để hiển thị bên phải)
        public List<FastFood> ListProducts { get; set; }

        // Lưu lại id loại đang chọn để tô màu đậm (active)
        public int? SelectedCategoryId { get; set; }
    }
}