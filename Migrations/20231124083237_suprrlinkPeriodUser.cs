using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacancyProAPI.Migrations
{
    /// <inheritdoc />
    public partial class suprrlinkPeriodUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Periods_AspNetUsers_CreatorId",
                table: "Periods");

            migrationBuilder.DropForeignKey(
                name: "FK_Periods_AspNetUsers_UserId",
                table: "Periods");

            migrationBuilder.DropIndex(
                name: "IX_Periods_UserId",
                table: "Periods");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Periods");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "Periods",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Periods_AspNetUsers_CreatorId",
                table: "Periods",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Periods_AspNetUsers_CreatorId",
                table: "Periods");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "Periods",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Periods",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Periods_UserId",
                table: "Periods",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Periods_AspNetUsers_CreatorId",
                table: "Periods",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Periods_AspNetUsers_UserId",
                table: "Periods",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
