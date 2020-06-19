using Coco.Common.Const;
using Coco.Contract.MapBuilder;
using Coco.Entities.Domain.Identity;
using LinqToDB.Mapping;

namespace Coco.IdentityDAL.Mapping
{
    public class UserAuthorizationPolicyMap : EntityTypeBuilder<UserAuthorizationPolicy>
    {
        public UserAuthorizationPolicyMap(FluentMappingBuilder fluentMappingBuilder) : base(fluentMappingBuilder)
        {
        }

        public override void Configure(FluentMappingBuilder builder)
        {
            builder.Entity<UserAuthorizationPolicy>().HasTableName(nameof(UserAuthorizationPolicy))
                .HasSchemaName(TableSchemaConst.DBO)
                .HasPrimaryKey(x => new
                {
                    x.UserId,
                    x.AuthorizationPolicyId
                })
                .Association(c => c.User, (userAuthorizationPolicy, user) => userAuthorizationPolicy.UserId == user.Id)
                .Association(c => c.GrantedBy, 
                    (userAuthorizationPolicy, grantedBy) => userAuthorizationPolicy.GrantedById == grantedBy.Id)
                .Association(c => c.AuthorizationPolicy,
                    (userAuthorizationPolicy, authorizationPolicy) => userAuthorizationPolicy.AuthorizationPolicyId == authorizationPolicy.Id);
        }
    }
}
