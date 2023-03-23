using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileShop.Models
{
    public class ProductModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public string BrandName { get; set; }
        public string Colors { get; set;}
        public string Reviews { get; set; }
        public string ReviewScore { get; set; }
        public string Price { get; set; }
        public string Stock { get; set; }
        public string Delivery { get; set; }
        public string SKU { get; set; }
        [NotMapped]
        public IFormFile PictureFile { get; set; }

    }
}
