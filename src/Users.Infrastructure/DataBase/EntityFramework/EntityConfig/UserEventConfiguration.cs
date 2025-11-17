using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Domain.Entities;

namespace Users.Infrastructure.DataBase.EntityFramework.EntityConfig
{
    public class UserEventConfiguration : IEntityTypeConfiguration<UserEvent>
    {
        public void Configure(EntityTypeBuilder<UserEvent> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
              .UseIdentityColumn(1, 1)
              .ValueGeneratedOnAdd();

            builder.Property(u => u.CreateAt).ValueGeneratedOnAdd();

            builder.Property(u => u.Event);

            builder.Property(u => u.email);

            builder.Property(u => u.UserId);

            builder.ToTable("UAC_Event_User");
        }
    }
}