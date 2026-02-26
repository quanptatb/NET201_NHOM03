using System.Collections.Generic;
using System.Linq;

namespace WebBanThucAnNhanh.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; } // Giá gốc món ăn
        public int Quantity { get; set; }

        // --- MỚI THÊM: Danh sách Topping khách chọn ---
        public List<CartTopping> SelectedToppings { get; set; } = new List<CartTopping>();

        // Tính tổng tiền Topping của 1 món
        public decimal ToppingPrice => SelectedToppings.Sum(t => t.Price);

        // Đơn giá 1 phần (Giá gốc + Giá Topping)
        public decimal UnitPrice => Price + ToppingPrice;

        // Tổng tiền của cả dòng (Đơn giá * Số lượng)
        public decimal Total => UnitPrice * Quantity;

        // --- THUẬT TOÁN NHẬN DIỆN GỘP/TÁCH ---
        public string CartItemKey
        {
            get
            {
                // Nếu không có topping, Key chính là Id món ăn
                if (SelectedToppings == null || !SelectedToppings.Any()) 
                    return Id.ToString();

                // Nếu có topping, nối Id món ăn với danh sách Id Topping đã sắp xếp
                // Ví dụ: Món 1, Topping 2 và 5 -> Key sẽ là "1_2_5"
                var toppingIds = SelectedToppings.Select(t => t.Id).OrderBy(id => id);
                return $"{Id}_{string.Join("_", toppingIds)}";
            }
        }
    }

    // Lớp phụ để lưu thông tin Topping ngay trong Giỏ hàng (Session)
    public class CartTopping
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}