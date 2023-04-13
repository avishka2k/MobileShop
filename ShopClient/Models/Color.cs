namespace ShopClient.Models
{
    public class Color
    {
        public int Id { get; set; }
        public string ColorCode { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
