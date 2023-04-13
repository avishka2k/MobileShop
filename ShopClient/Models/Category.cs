using System.ComponentModel.DataAnnotations.Schema;

namespace ShopClient.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
        public string PictureUrl { get; set; }
        [NotMapped]
        public IFormFile PictureFile { get; set; }
    }
}
