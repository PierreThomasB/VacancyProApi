using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacancyProAPI.Migrations
{
    /// <inheritdoc />
    public partial class removeLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Periods_PeriodId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Periods_AspNetUsers_CreatorId",
                table: "Periods");

            migrationBuilder.DropIndex(
                name: "IX_Periods_CreatorId",
                table: "Periods");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PeriodId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Periods");

            migrationBuilder.DropColumn(
                name: "PeriodId",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "Periods",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PeriodId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Periods_CreatorId",
                table: "Periods",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PeriodId",
                table: "AspNetUsers",
                column: "PeriodId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Periods_PeriodId",
                table: "AspNetUsers",
                column: "PeriodId",
                principalTable: "Periods",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Periods_AspNetUsers_CreatorId",
                table: "Periods",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
