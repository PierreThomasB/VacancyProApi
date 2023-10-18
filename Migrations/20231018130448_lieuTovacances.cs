using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacancyProAPI.Migrations
{
    /// <inheritdoc />
    public partial class lieuTovacances : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LieuxIdLieu",
                table: "Vacances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Vacances_LieuxIdLieu",
                table: "Vacances",
                column: "LieuxIdLieu");

            migrationBuilder.AddForeignKey(
                name: "FK_Vacances_Lieux_LieuxIdLieu",
                table: "Vacances",
                column: "LieuxIdLieu",
                principalTable: "Lieux",
                principalColumn: "IdLieu",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vacances_Lieux_LieuxIdLieu",
                table: "Vacances");

            migrationBuilder.DropIndex(
                name: "IX_Vacances_LieuxIdLieu",
                table: "Vacances");

            migrationBuilder.DropColumn(
                name: "LieuxIdLieu",
                table: "Vacances");
        }
    }
}
