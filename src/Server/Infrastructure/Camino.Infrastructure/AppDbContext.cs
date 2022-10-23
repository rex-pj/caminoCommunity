using Camino.Application.Contracts;
using Camino.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Camino.Infrastructure
{
    public class AppDbContext : CaminoDbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<CaminoDbContext> options) : base(options)
        {
        }
    }
}
