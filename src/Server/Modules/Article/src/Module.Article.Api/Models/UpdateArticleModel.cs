using System.ComponentModel.DataAnnotations;

namespace Module.Article.Api.Models
{
    public class UpdateArticleModel
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public int ArticleCategoryId { get; set; }
    }
}
