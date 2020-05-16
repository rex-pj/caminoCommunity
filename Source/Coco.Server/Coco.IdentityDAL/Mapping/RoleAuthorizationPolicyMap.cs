using Coco.Entities.Constant;
using Coco.Entities.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coco.IdentityDAL.Mapping
{
    public class RoleAuthorizationPolicyMap : IEntityTypeConfiguration<RoleAuthorizationPolicy>
    {
        public void Configure(EntityTypeBuilder<RoleAuthorizationPolicy> builder)
        {
            builder.ToTable(nameof(RoleAuthorizationPolicy), TableSchemaConst.DBO);
            builder.HasKey(x => new
            {
                x.RoleId,
                x.AuthorizationPolicyId
            });
        }
    }
}
