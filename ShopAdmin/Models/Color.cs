namespace ShopAdmin.Models
{
    public class Color
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
