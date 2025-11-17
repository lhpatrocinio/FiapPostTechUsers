using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Users.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlteradoUserEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UAC_Event_User_UAC_User_UserId",
                table: "UAC_Event_User");

            migrationBuilder.DropIndex(
                name: "IX_UAC_Event_User_UserId",
                table: "UAC_Event_User");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UAC_Event_User_UserId",
                table: "UAC_Event_User",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UAC_Event_User_UAC_User_UserId",
                table: "UAC_Event_User",
                column: "UserId",
                principalTable: "UAC_User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
