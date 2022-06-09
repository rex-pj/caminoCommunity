namespace Camino.Core.Domains.Articles.Repositories
{
    public interface IArticleRepository
    {
        Task<long> CreateAsync(Article article);
        Task<Article> FindAsync(long id);
        Task<Article> FindByNameAsync(string name);
        Task<bool> UpdateAsync(Article article);
        Task<bool> DeleteAsync(long id);
    }
}