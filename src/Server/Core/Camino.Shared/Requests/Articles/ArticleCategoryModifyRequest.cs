namespace Camino.Shared.Requests.Articles
{
    public class ArticleCategoryModifyRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long UpdatedById { get; set; }
        public long CreatedById { get; set; }
        public int? ParentId { get; set; }
        public string ParentCategoryName { get; set; }
    }
}
