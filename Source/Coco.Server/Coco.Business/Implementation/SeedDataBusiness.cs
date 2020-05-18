using Coco.Business.Contracts;
using Coco.IdentityDAL;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Coco.Business.Implementation
{
    public class SeedDataBusiness : ISeedDataBusiness
    {
        private readonly IdentityDbContext _identityDbContext;
        public SeedDataBusiness(IdentityDbContext identityDbContext)
        {
            _identityDbContext = identityDbContext;
        }

        public bool CanSeed()
        {
            return !_identityDbContext.Database.GetService<IRelationalDatabaseCreator>().Exists();
        }

        public void SeedingData()
        {
            if (!CanSeed())
            {
                return;
            }
            _identityDbContext.Database.EnsureCreated();
        }
    }
}
