using System;

namespace Camino.Application.Contracts.AppServices.Articles.Dtos
{
    public class ArticleCategoryResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long UpdatedById { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public int? ParentId { get; set; }
        public string ParentCategoryName { get; set; }
        public ArticleCategoryResult ParentCategory { get; set; }
        public int StatusId { get; set; }

        public IEnumerable<ArticleCategoryResult> ChildCategories { get; set; }
    }
}
