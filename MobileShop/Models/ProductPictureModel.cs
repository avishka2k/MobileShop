namespace MobileShop.Models
{
    public class ProductPictureModel
    {
        public Guid Id { get; set; }
        public string ProductPictureUrl { get; set; }

        public ProductModel Product { get; set; }

    }
}
