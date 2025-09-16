using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Domain.Entities.Identity;

namespace Core.PosTech8Nett.Api.Infra.DataBase.EntityFramework.EntityConfig.Identity
{
    public class IdentityRoleConfiguration : IEntityTypeConfiguration<Roles>
    {
        public void Configure(EntityTypeBuilder<Roles> builder)
        {
            builder.ToTable("UAC_Roles");
        }

    }
}
