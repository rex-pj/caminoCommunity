using Coco.Entities.Constant;
using Coco.Entities.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coco.IdentityDAL.Mapping
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(UserInfo), TableSchemaConst.DBO);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder
                .HasOne(x => x.UserInfo)
                .WithOne(x => x.User)
                .HasForeignKey<UserInfo>(x => x.Id);

            builder
                .HasMany(x => x.CreatedUsers)
                .WithOne(x => x.CreatedBy)
                .HasForeignKey(x => x.CreatedById);

            builder
                .HasMany(x => x.UpdatedUsers)
                .WithOne(x => x.UpdatedBy)
                .HasForeignKey(x => x.UpdatedById);

            builder
                .HasMany(c => c.UserRoles)
               .WithOne(x => x.User)
               .HasForeignKey(c => c.UserId);

            builder
               .HasMany(c => c.CreatedRoles)
               .WithOne(x => x.CreatedBy)
               .HasForeignKey(c => c.CreatedById);

            builder
               .HasMany(c => c.UpdatedRoles)
               .WithOne(x => x.UpdatedBy)
               .HasForeignKey(c => c.UpdatedById);

            builder
                .HasMany(c => c.CreatedAuthorizationPolicies)
                .WithOne(x => x.CreatedBy)
                .HasForeignKey(c => c.CreatedById);

            builder
                .HasMany(c => c.UpdatedAuthorizationPolicies)
                .WithOne(x => x.UpdatedBy)
                .HasForeignKey(c => c.UpdatedById);

            builder
                .HasMany(c => c.UserAuthorizationPolicies)
                .WithOne(x => x.User)
                .HasForeignKey(c => c.UserId);

            builder
                .HasMany(c => c.UserAuthorizationPolicies)
                .WithOne(x => x.User)
                .HasForeignKey(c => c.UserId);

            builder
                .HasMany(c => c.GrantedRoleAuthorizationPolicies)
                .WithOne(x => x.GrantedBy)
                .HasForeignKey(c => c.GrantedById);
        }
    }
}
