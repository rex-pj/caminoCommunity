namespace Camino.Core.Domains.Articles.Repositories
{
    public interface IArticleCategoryRepository
    {
        Task<ArticleCategory> FindAsync(int id);
        Task<int> CreateAsync(ArticleCategory category);
        Task<bool> UpdateAsync(ArticleCategory category);
        Task<ArticleCategory> FindByNameAsync(string name);
        Task<bool> DeleteAsync(int id);
    }
}
