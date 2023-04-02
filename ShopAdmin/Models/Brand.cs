using System.ComponentModel.DataAnnotations.Schema;

namespace ShopAdmin.Models
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
        public string PictureUrl { get; set; }
        [NotMapped]
        public IFormFile PictureFile { get; set; }
    }
}
