using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Domain.Entities.Identity;

namespace Core.PosTech8Nett.Api.Infra.DataBase.EntityFramework.EntityConfig.Identity
{
    public class IdentityUserLoginConfiguration : IEntityTypeConfiguration<UserLogins>
    {
        public void Configure(EntityTypeBuilder<UserLogins> builder)
        {
            builder.HasKey(u => new { u.LoginProvider, u.ProviderKey, u.UserId });

            builder.Property(u => u.UserId);

            builder.ToTable("UAC_UserLogins");
        }
    }
}
