using Camino.Core.Constants;
using Camino.Data.MapBuilders;
using Camino.IdentityDAL.Entities;
using LinqToDB.Mapping;

namespace Camino.IdentityDAL.Mapping
{
    public class UserAuthorizationPolicyMap : EntityMapBuilder<UserAuthorizationPolicy>
    {
        public override void Map(FluentMappingBuilder builder)
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
