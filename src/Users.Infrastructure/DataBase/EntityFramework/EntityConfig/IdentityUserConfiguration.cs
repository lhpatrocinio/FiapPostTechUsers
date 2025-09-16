using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Domain.Entities.Identity;

namespace Core.PosTech8Nett.Api.Infra.DataBase.EntityFramework.EntityConfig.Identity
{
    public class IdentityUserConfiguration : IEntityTypeConfiguration<UsersEntitie>
    {
        public void Configure(EntityTypeBuilder<UsersEntitie> builder)
        {
            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(u => u.FirstName)
               .HasMaxLength(100);

            builder.Property(u => u.LastName)
             .HasMaxLength(200);

            builder.Property(u => u.NickName)
             .HasMaxLength(50);

            builder.Property(u => u.Birthdate).HasColumnType("DATE");

            builder.Property(u => u.CreatedAt).ValueGeneratedOnAdd();

            builder.Property(u => u.UpdateAt);

            builder.Ignore(u => u.PhoneNumber);
            builder.Ignore(u => u.PhoneNumberConfirmed);

            builder.ToTable("UAC_User");
        }
    }
}
