using Coco.Contract;
using Coco.Core.Infrastructure.MapBuilders;
using Coco.IdentityDAL.Contracts;
using Coco.IdentityDAL.Mapping;

namespace Coco.IdentityDAL.Implementations
{
    public class IdentityDataProvider : BaseDataProvider<IdentityMappingSchema>, IIdentityDataProvider
    {
        public IdentityDataProvider(IdentityDbConnection dataConnection) : base(dataConnection)
        {

        }

        protected override void OnMappingSchemaCreating()
        {
            FluentMappingBuilder.ApplyMappingBuilder<UserMap>()
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
