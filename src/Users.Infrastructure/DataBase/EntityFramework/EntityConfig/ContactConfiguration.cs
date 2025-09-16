using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Users.Domain.Entities;

namespace Users.Infrastructure.DataBase.EntityFramework.EntityConfig
{
    public class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
              .UseIdentityColumn(1, 1)
              .ValueGeneratedOnAdd();

            builder.Property(u => u.IdContactType).HasConversion<int>();

            builder.Property(u => u.Description)
               .HasMaxLength(100);

            builder.Property(u => u.CreateAt).ValueGeneratedOnAdd();

            builder.Property(u => u.UpdateAt).ValueGeneratedOnAddOrUpdate();

            builder.HasOne(a => a.User)
                   .WithOne(u => u.Contact)
                   .HasForeignKey<Contact>(a => a.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("UAC_Contact");
        }
    }
}
