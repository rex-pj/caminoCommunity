using Camino.Data.Contracts;
using Camino.Data.MapBuilders;
using Camino.IdentityDAL.Contracts;
using Camino.IdentityDAL.Mapping;

namespace Camino.IdentityDAL.Implementations
{
    public class IdentityDataProvider : BaseDataProvider<IdentityMappingSchema>, IIdentityDataProvider
    {
        public IdentityDataProvider(IdentityDbConnection dataConnection) : base(dataConnection)
        {

        }

        protected override void OnMappingSchemaCreating()
        {
            FluentMapBuilder.ApplyMappingBuilder<UserMap>()
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
                .ApplyMappingBuilder<UserTokenMap>();
        }
    }
}
