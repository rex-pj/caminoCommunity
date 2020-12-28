using Camino.DAL.Contracts;
using Camino.DAL.Entities;
using Camino.Service.Business.Setup.Contracts;
using Camino.Service.Projections.Request;
using LinqToDB;
using System;
using System.Threading.Tasks;

namespace Camino.Service.Business.Setup
{
    public class ContentDataSetupBusiness : IContentDataSetupBusiness
    {
        private readonly IContentDataProvider _contentDataProvider;
        private readonly ISeedDataBusiness _seedDataBusiness;
        public ContentDataSetupBusiness(IContentDataProvider contentDataProvider, ISeedDataBusiness seedDataBusiness)
        {
            _contentDataProvider = contentDataProvider;
            _seedDataBusiness = seedDataBusiness;
        }

        public bool IsContentDatabaseExist()
        {
            return _contentDataProvider.IsDatabaseExist();
        }

        public void SeedingContentDb(string sql)
        {
            if (IsContentDatabaseExist())
            {
                return;
            }

            _seedDataBusiness.CreateDatabase(_contentDataProvider);
            _seedDataBusiness.CreateDataByScript(_contentDataProvider, sql);
        }

        public async Task PrepareContentDataAsync(SetupRequest installationRequest)
        {
            using (var dataConnection = _contentDataProvider.CreateDataConnection())
            {
                using (var transaction = dataConnection.BeginTransaction())
                {

                    try
                    {
                        // Insert countries
                        var userPhotoTypeTableName = nameof(UserPhotoType);
                        foreach (var userPhotoType in installationRequest.UserPhotoTypes)
                        {
                            await dataConnection.InsertAsync(new UserPhotoType()
                            {
                                Name = userPhotoType.Name,
                                Description = userPhotoType.Description
                            }, userPhotoTypeTableName);
                        }

                        await transaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                    }
                }
            }
        }
    }
}
