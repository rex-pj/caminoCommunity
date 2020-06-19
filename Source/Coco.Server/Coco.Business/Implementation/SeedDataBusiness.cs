using Coco.Business.Contracts;
using Coco.IdentityDAL;
using System;

namespace Coco.Business.Implementation
{
    public class SeedDataBusiness : ISeedDataBusiness
    {
        private readonly IdentityDbConnection _identityDbContext;
        public SeedDataBusiness(IdentityDbConnection identityDbContext)
        {
            _identityDbContext = identityDbContext;
        }

        public bool CanSeed()
        {
            throw new NotImplementedException();
            //return !_identityDbContext.Database.GetService<IRelationalDatabaseCreator>().Exists();
        }

        public void SeedingData()
        {
            if (!CanSeed())
            {
                return;
            }

            throw new NotImplementedException();
            //_identityDbContext.Database.EnsureCreated();
        }
    }
}
