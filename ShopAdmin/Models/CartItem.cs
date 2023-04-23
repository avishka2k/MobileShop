namespace ShopAdmin.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public Order Order { get; set; }
        public int OrderId { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public string Color { get; set; }
        public int Quantity { get; set; }
    }
}