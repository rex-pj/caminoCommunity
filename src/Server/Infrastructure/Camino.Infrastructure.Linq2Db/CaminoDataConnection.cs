using LinqToDB.Configuration;
using Camino.Infrastructure.Linq2Db.MapBuilders;
using Camino.Infrastructure.Linq2Db.Mapping.Articles;
using Camino.Infrastructure.Linq2Db.Mapping.Farms;
using Camino.Infrastructure.Linq2Db.Mapping.Identities;
using Camino.Infrastructure.Linq2Db.Mapping.Media;
using Camino.Infrastructure.Linq2Db.Mapping.Products;
using Camino.Infrastructure.Linq2Db.Mapping.Navigations;

namespace Camino.Infrastructure.Linq2Db
{
    public class CaminoDataConnection : SqlServerDataConnection
    {
        public CaminoDataConnection(LinqToDbConnectionOptions<CaminoDataConnection> options) : base(options)
        {
        }

        protected override void OnMappingSchemaCreating()
        {
            FluentMapBuilder.ApplyMappingBuilder<UserPhotoMap>()
                .ApplyMappingBuilder<UserPhotoTypeMap>()
                .ApplyMappingBuilder<ArticleMap>()
                .ApplyMappingBuilder<ArticleCategoryMap>()
                .ApplyMappingBuilder<PictureMap>()
                .ApplyMappingBuilder<ArticlePictureMap>()
                .ApplyMappingBuilder<FarmMap>()
                .ApplyMappingBuilder<FarmTypeMap>()
                .ApplyMappingBuilder<FarmPictureMap>()
                .ApplyMappingBuilder<ProductMap>()
                .ApplyMappingBuilder<ProductCategoryMap>()
                .ApplyMappingBuilder<ProductPictureMap>()
                .ApplyMappingBuilder<ProductCategoryRelationMap>()
                .ApplyMappingBuilder<ProductPriceMap>()
                .ApplyMappingBuilder<OrderMap>()
                .ApplyMappingBuilder<OrderItemMap>()
                .ApplyMappingBuilder<FarmProductMap>()
                .ApplyMappingBuilder<ProductAttributeMap>()
                .ApplyMappingBuilder<ProductAttributeRelationMap>()
                .ApplyMappingBuilder<ProductAttributeRelationValueMap>()
                .ApplyMappingBuilder<UserMap>()
                .ApplyMappingBuilder<UserInfoMap>()
                .ApplyMappingBuilder<AuthorizationPolicyMap>()
                .ApplyMappingBuilder<CountryMap>()
                .ApplyMappingBuilder<GenderMap>()
                .ApplyMappingBuilder<RoleAuthorizationPolicyMap>()
                .ApplyMappingBuilder<RoleClaimMap>()
                .ApplyMappingBuilder<RoleMap>()
                .ApplyMappingBuilder<StatusMap>()
                .ApplyMappingBuilder<UserAttributeMap>()
                .ApplyMappingBuilder<UserAuthorizationPolicyMap>()
                .ApplyMappingBuilder<UserClaimMap>()
                .ApplyMappingBuilder<UserLoginMap>()
                .ApplyMappingBuilder<UserRoleMap>()
                .ApplyMappingBuilder<UserTokenMap>()
                .ApplyMappingBuilder<ShortcutMap>();
        }
    }
}
