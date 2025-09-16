using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Domain.Entities.Identity;

namespace Core.PosTech8Nett.Api.Infra.DataBase.EntityFramework.EntityConfig.Identity
{
    public class IdentityUserRoleConfiguration : IEntityTypeConfiguration<UserRoles>
    {
        public void Configure(EntityTypeBuilder<UserRoles> builder)
        {
            builder.HasKey(u => new { u.UserId, u.RoleId });

            builder.HasOne(ur => ur.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.UserId);

            builder.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId);

            builder.ToTable("UAC_UserRoles");
        }
    }
}
