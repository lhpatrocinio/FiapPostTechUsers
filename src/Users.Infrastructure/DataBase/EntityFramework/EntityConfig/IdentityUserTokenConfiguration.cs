using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Domain.Entities.Identity;

namespace Core.PosTech8Nett.Api.Infra.DataBase.EntityFramework.EntityConfig.Identity
{
    public class IdentityUserTokenConfiguration : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            builder.ToTable("UAC_UserToken");
        }
    }
}
