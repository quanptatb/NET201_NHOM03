namespace WebBanThucAnNhanh.Models
{
    public class CartItemOption
    {
        public int OptionItemId { get; set; }
        public string OptionName { get; set; }
        public decimal AdditionalPrice { get; set; }
    }
    public class CartItem
    {
        public int Id { get; set; }
        public string CartItemSignature =>
            $"{Id}_{string.Join("_", SelectedOptions.OrderBy(o => o.OptionItemId).Select(o => o.OptionItemId))}";
        public string Name { get; set; }
        public string Image { get; set; }
        public decimal BasePrice { get; set; }
        // Danh sách topping/size đã chọn
        public List<CartItemOption> SelectedOptions { get; set; } = new List<CartItemOption>();

        public decimal Price => BasePrice + SelectedOptions.Sum(o => o.AdditionalPrice);

        public int Quantity { get; set; }

        // Nếu có thuộc tính Total, cũng đổi thành decimal
        public decimal Total => Price * Quantity;
    }
}