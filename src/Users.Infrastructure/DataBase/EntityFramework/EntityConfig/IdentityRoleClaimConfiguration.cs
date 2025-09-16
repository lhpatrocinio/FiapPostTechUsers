using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Domain.Entities.Identity;

namespace Core.PosTech8Nett.Api.Infra.DataBase.EntityFramework.EntityConfig.Identity
{
    public class IdentityRoleClaimConfiguration : IEntityTypeConfiguration<Claims>
    {
        public void Configure(EntityTypeBuilder<Claims> builder)
        {
            builder.ToTable("UAC_Claims");
        }
    }
}
