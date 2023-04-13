namespace ShopClient.Models
{
    public class CartItem
    {
     
        public Product Product { get; set; }
        public string ProductTitle { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
    }
}
