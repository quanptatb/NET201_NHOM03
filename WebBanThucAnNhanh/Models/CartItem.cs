namespace WebBanThucAnNhanh.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }

        // SỬA DÒNG NÀY: đổi double thành decimal
        public decimal Price { get; set; }

        public int Quantity { get; set; }

        // Nếu có thuộc tính Total, cũng đổi thành decimal
        public decimal Total => Price * Quantity;
    }
}