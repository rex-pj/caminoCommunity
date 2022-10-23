using System;
using System.Threading.Tasks;
using Camino.Core.Domains.Articles.Repositories;
using Camino.Core.Domains.Articles;
using Camino.Core.Domains;
using Camino.Core.DependencyInjection;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Articles
{
    public class ArticleRepository : IArticleRepository, IScopedDependency
    {
        private readonly IEntityRepository<Article> _articleRepository;
        private readonly IDbContext _dbContext;

        public ArticleRepository(IEntityRepository<Article> articleRepository,
            IDbContext dbContext)
        {
            _articleRepository = articleRepository;
            _dbContext = dbContext;
        }

        public async Task<Article> FindAsync(long id)
        {
            var exist = await _articleRepository.FindAsync(x => x.Id == id);
            return exist;
        }

        public async Task<Article> FindByNameAsync(string name)
        {
            var article = await _articleRepository.FindAsync(x => x.Name == name);
            return article;
        }

        public async Task<long> CreateAsync(Article article)
        {
            var modifiedDate = DateTime.UtcNow;
            article.CreatedDate = modifiedDate;
            article.UpdatedDate = modifiedDate;

            await _articleRepository.InsertAsync(article);
            await _dbContext.SaveChangesAsync();
            return article.Id;
        }

        public async Task<bool> UpdateAsync(Article article)
        {
            var modifiedDate = DateTime.UtcNow;
            article.UpdatedDate = modifiedDate;

            await _articleRepository.UpdateAsync(article);
            return (await _dbContext.SaveChangesAsync()) > 0;
        }


        public async Task<bool> DeleteAsync(long id)
        {
            await _articleRepository.DeleteAsync(x => x.Id == id);
            return (await _dbContext.SaveChangesAsync()) > 0;
        }
    }
}
