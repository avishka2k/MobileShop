namespace MobileShop.Models
{
    public class TagsModel
    {
        public int Id { get; set; }
        public string TagName { get; set; }
        public List<TagsModel> GetTagsModels { get; set; }
    }
}
