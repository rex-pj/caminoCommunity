using System;
using System.Linq;
using System.Threading.Tasks;
using Camino.Core.Domains.Articles.Repositories;
using Camino.Core.Domains.Articles;
using Microsoft.EntityFrameworkCore;
using Camino.Core.Domains;
using Camino.Core.DependencyInjection;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Articles
{
    public class ArticleCategoryRepository : IArticleCategoryRepository, IScopedDependency
    {
        private readonly IEntityRepository<ArticleCategory> _articleCategoryRepository;
        private readonly IAppDbContext _dbContext;

        public ArticleCategoryRepository(IEntityRepository<ArticleCategory> articleCategoryRepository, IAppDbContext dbContext)
        {
            _articleCategoryRepository = articleCategoryRepository;
            _dbContext = dbContext;
        }

        public async Task<ArticleCategory> FindAsync(int id)
        {
            var category = await (from child in _articleCategoryRepository.Table
                                  join parent in _articleCategoryRepository.Table
                                  on child.ParentId equals parent.Id into categories
                                  from cate in categories.DefaultIfEmpty()
                                  where child.Id == id
                                  select new ArticleCategory
                                  {
                                      Description = child.Description,
                                      CreatedDate = child.CreatedDate,
                                      CreatedById = child.CreatedById,
                                      Id = child.Id,
                                      Name = child.Name,
                                      ParentId = child.ParentId,
                                      UpdatedById = child.UpdatedById,
                                      UpdatedDate = child.UpdatedDate,
                                      StatusId = child.StatusId,
                                      ParentCategory = cate
                                  }).FirstOrDefaultAsync();

            return category;
        }

        public async Task<ArticleCategory> FindByNameAsync(string name)
        {
            var category = await _articleCategoryRepository.Get(x => x.Name == name)
                .Select(x => new ArticleCategory
                {
                    CreatedById = x.CreatedById,
                    CreatedDate = x.CreatedDate,
                    Description = x.Description,
                    Id = x.Id,
                    Name = x.Name,
                    ParentId = x.ParentId,
                    UpdatedById = x.UpdatedById,
                    UpdatedDate = x.UpdatedDate,
                    StatusId = x.StatusId
                })
                .FirstOrDefaultAsync();

            return category;
        }

        

        public async Task<int> CreateAsync(ArticleCategory category)
        {
            var modifiedDate = DateTime.UtcNow;
            category.CreatedDate = modifiedDate;
            category.UpdatedDate = modifiedDate;
            await _articleCategoryRepository.InsertAsync(category);
            await _dbContext.SaveChangesAsync();
            return category.Id;
        }

        public async Task<bool> UpdateAsync(ArticleCategory category)
        {
            category.UpdatedDate = DateTime.UtcNow;
            return (await _dbContext.SaveChangesAsync()) > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedNumbers = await _articleCategoryRepository.DeleteAsync(x => x.Id == id);
            return (await _dbContext.SaveChangesAsync()) > 0;
        }
    }
}
