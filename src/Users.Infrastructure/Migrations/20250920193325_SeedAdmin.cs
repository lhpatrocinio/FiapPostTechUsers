using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Users.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //email: admin@fiap.com
            //senha: Fiap2025@
            migrationBuilder.Sql(@"
                  INSERT [dbo].[UAC_User] ([Id], [FirstName], [LastName], [Birthdate], [NickName], [CreatedAt], [UpdateAt], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) 
                  VALUES (N'991986d7-6621-4645-b3f3-1f3a7e444e13', 'Administrator', 'Fiap', CAST(N'0001-01-01' AS Date), 'Admin', CAST(N'2025-05-21T01:13:09.8377003' AS DateTime2), CAST(N'2025-05-21T01:13:09.8377983' AS DateTime2), N'admin@fiap.com', N'ADMIN@FIAP.COM', N'admin@fiap.com', N'ADMIN@FIAP.COM', 1, N'AQAAAAIAAYagAAAAEDIRkvTV/t16Db74P8MsJxvC0XHtFSHKQ4Pjc/513Y7EMIM7+iijS0MYwnopxAjBvw==', N'DOISCTYC74I3SI72F65FCDYEXGOGPHPZ', N'04fbbb10-453d-452e-9bf8-58eb9dff5c89', 0, NULL, 1, 0)
                  
                  INSERT [dbo].[UAC_UserRoles] ([RoleId], [UserId] )
                  VALUES (N'8947bbc5-010c-4df8-93f3-35c42cddca40', N'991986d7-6621-4645-b3f3-1f3a7e444e13')

                  INSERT [dbo].[UAC_UserRoles] ([RoleId], [UserId] )
                  VALUES (N'574f0964-e04f-4e3d-9788-108e90664955', N'991986d7-6621-4645-b3f3-1f3a7e444e13')

                  INSERT [dbo].[UAC_UserRoles] ([RoleId], [UserId] )
                  VALUES (N'a17d36de-fdd7-4a58-8c90-cf679d4f1805', N'991986d7-6621-4645-b3f3-1f3a7e444e13')
                ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM [dbo].[UAC_User] WHERE UserName IN (
                    'admin@fiap.com'
                    );
                ");
        }
    }
}
