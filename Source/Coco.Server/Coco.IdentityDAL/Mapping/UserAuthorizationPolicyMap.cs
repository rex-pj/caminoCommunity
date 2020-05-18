using Coco.Common.Const;
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

            builder
               .HasOne(c => c.User)
               .WithMany(x => x.UserAuthorizationPolicies)
               .HasForeignKey(c => c.UserId)
               .OnDelete(DeleteBehavior.NoAction);

            builder
               .HasOne(c => c.GrantedBy)
               .WithMany(x => x.GrantedUserAuthorizationPolicies)
               .HasForeignKey(c => c.GrantedById)
               .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
