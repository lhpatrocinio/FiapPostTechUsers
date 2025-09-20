using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Users.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                   INSERT [dbo].[UAC_Roles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) 
                   VALUES (N'4a226e7c-fb05-4919-a5f0-136bf0633fc3', N'Usuario', N'USUARIO', N'cb5f9652-fc9b-45ea-9ff2-e0d25a4cf611')

                   INSERT [dbo].[UAC_Roles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) 
                   VALUES (N'8947bbc5-010c-4df8-93f3-35c42cddca40', N'Admin', N'ADMIN', N'724786ea-4985-4d67-9beb-67b9790260ef')
                ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM [dbo].[UAC_Roles] WHERE Name IN (
                    'Usuario',
                    'Admin'
                    );
                ");
        }
    }
}
