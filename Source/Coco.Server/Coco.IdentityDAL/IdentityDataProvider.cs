using Coco.Contract;
using Coco.Contract.MapBuilder;
using Coco.IdentityDAL.Mapping;

namespace Coco.IdentityDAL
{
    public class IdentityDataProvider : DataProvider, IDataProvider
    {
        public IdentityDataProvider(IdentityDbConnection dataConnection) : base(dataConnection)
        {
        }

        protected override void OnMappingSchemaCreating(MappingSchemaBuilder builder)
        {
            var fluentBuilder = builder.FluentMappingBuilder;
            builder.ApplyMappingBuilder(new UserMap(builder.FluentMappingBuilder));
            builder.ApplyMappingBuilder(new UserInfoMap(fluentBuilder));
            builder.ApplyMappingBuilder(new AuthorizationPolicyMap(fluentBuilder));
            builder.ApplyMappingBuilder(new CountryMap(fluentBuilder));
            builder.ApplyMappingBuilder(new GenderMap(fluentBuilder));
            builder.ApplyMappingBuilder(new RoleAuthorizationPolicyMap(fluentBuilder));
            builder.ApplyMappingBuilder(new RoleClaimMap(fluentBuilder));
            builder.ApplyMappingBuilder(new RoleMap(fluentBuilder));
            builder.ApplyMappingBuilder(new StatusMap(fluentBuilder));
            builder.ApplyMappingBuilder(new UserAttributeMap(fluentBuilder));
            builder.ApplyMappingBuilder(new UserAuthorizationPolicyMap(fluentBuilder));
            builder.ApplyMappingBuilder(new UserClaimMap(fluentBuilder));
            builder.ApplyMappingBuilder(new UserLoginMap(fluentBuilder));
            builder.ApplyMappingBuilder(new UserRoleMap(fluentBuilder));
            builder.ApplyMappingBuilder(new UserTokenMap(fluentBuilder));

            base.OnMappingSchemaCreating(builder);
        }
    }
}
