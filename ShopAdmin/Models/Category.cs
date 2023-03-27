using System.ComponentModel.DataAnnotations.Schema;

namespace ShopAdmin.Models
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string? Description { get; set; }
        public string PictureUrl { get; set; }
        [NotMapped]
        public IFormFile PictureFile { get; set; }
    }
}
