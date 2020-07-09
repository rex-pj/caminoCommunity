using Coco.Contract;
using Coco.Contract.MapBuilder;
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
            var fluentBuilder = MappingSchemaBuilder.FluentMappingBuilder;
            MappingSchemaBuilder.ApplyMappingBuilder(new UserMap(fluentBuilder));
            MappingSchemaBuilder.ApplyMappingBuilder(new UserInfoMap(fluentBuilder));
            MappingSchemaBuilder.ApplyMappingBuilder(new AuthorizationPolicyMap(fluentBuilder));
            MappingSchemaBuilder.ApplyMappingBuilder(new CountryMap(fluentBuilder));
            MappingSchemaBuilder.ApplyMappingBuilder(new GenderMap(fluentBuilder));
            MappingSchemaBuilder.ApplyMappingBuilder(new RoleAuthorizationPolicyMap(fluentBuilder));
            MappingSchemaBuilder.ApplyMappingBuilder(new RoleClaimMap(fluentBuilder));
            MappingSchemaBuilder.ApplyMappingBuilder(new RoleMap(fluentBuilder));
            MappingSchemaBuilder.ApplyMappingBuilder(new StatusMap(fluentBuilder));
            MappingSchemaBuilder.ApplyMappingBuilder(new UserAttributeMap(fluentBuilder));
            MappingSchemaBuilder.ApplyMappingBuilder(new UserAuthorizationPolicyMap(fluentBuilder));
            MappingSchemaBuilder.ApplyMappingBuilder(new UserClaimMap(fluentBuilder));
            MappingSchemaBuilder.ApplyMappingBuilder(new UserLoginMap(fluentBuilder));
            MappingSchemaBuilder.ApplyMappingBuilder(new UserRoleMap(fluentBuilder));
            MappingSchemaBuilder.ApplyMappingBuilder(new UserTokenMap(fluentBuilder));
        }
    }
}
