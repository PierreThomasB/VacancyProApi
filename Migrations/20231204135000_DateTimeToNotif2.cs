using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacancyProAPI.Migrations
{
    /// <inheritdoc />
    public partial class DateTimeToNotif2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_userId",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Notifications",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "Notifications",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "contenu",
                table: "Notifications",
                newName: "Contenu");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_userId",
                table: "Notifications",
                newName: "IX_Notifications_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_UserId",
                table: "Notifications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_UserId",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Notifications",
                newName: "userId");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Notifications",
                newName: "date");

            migrationBuilder.RenameColumn(
                name: "Contenu",
                table: "Notifications",
                newName: "contenu");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                newName: "IX_Notifications_userId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_userId",
                table: "Notifications",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
