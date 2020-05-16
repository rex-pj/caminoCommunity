using Coco.Entities.Constant;
using Coco.Entities.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coco.IdentityDAL.Mapping
{
    public class UserAuthorizationPolicyMap : IEntityTypeConfiguration<UserAuthorizationPolicy>
    {
        public void Configure(EntityTypeBuilder<UserAuthorizationPolicy> builder)
        {
            builder.ToTable(nameof(UserAuthorizationPolicy), TableSchemaConst.DBO);
            builder.HasKey(x => new
            {
                x.UserId,
                x.AuthorizationPolicyId
            });
        }
    }
}
