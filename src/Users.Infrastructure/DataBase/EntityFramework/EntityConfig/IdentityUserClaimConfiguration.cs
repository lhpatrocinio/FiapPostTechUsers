using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Domain.Entities.Identity;

namespace Core.PosTech8Nett.Api.Infra.DataBase.EntityFramework.EntityConfig.Identity
{
    public class IdentityUserClaimConfiguration : IEntityTypeConfiguration<UserClaims>
    {
        public void Configure(EntityTypeBuilder<UserClaims> builder)
        {
            builder.ToTable("UAC_UserClaims");
        }
    }
}
