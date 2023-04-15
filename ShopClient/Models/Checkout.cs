namespace ShopClient.Models
{
    public class Checkout
    {
        public List<CartItem> Cart { get; set; }
        public double TotalPrice { get; set; }
        public double Delivery { get; set; }
    }
}
