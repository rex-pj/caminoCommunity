using Camino.Infrastructure.Commons.Constants;
using Camino.Infrastructure.MapBuilders;
using Camino.Core.Domain.Identifiers;
using LinqToDB.Mapping;

namespace Camino.Infrastructure.Mapping.Identities
{
    public class UserAuthorizationPolicyMap : EntityMapBuilder<UserAuthorizationPolicy>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<UserAuthorizationPolicy>().HasTableName(nameof(UserAuthorizationPolicy))
                .HasSchemaName(TableSchemaConst.Auth)
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
