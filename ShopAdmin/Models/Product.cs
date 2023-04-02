
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopAdmin.Models

{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Colors { get; set; }
        public string Reviews { get; set; }
        public string ReviewScore { get; set; }
        public string Price { get; set; }
        public string Stock { get; set; }
        public string Delivery { get; set; }
        public string SKU { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        public List<Image> Images { get; set; }
        public string FirstImageUrl { get; set; }
    }
}