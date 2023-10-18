using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacancyProAPI.Migrations
{
    /// <inheritdoc />
    public partial class lieuTovacances2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VacancesIdVacances",
                table: "Activites",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Activites_VacancesIdVacances",
                table: "Activites",
                column: "VacancesIdVacances");

            migrationBuilder.AddForeignKey(
                name: "FK_Activites_Vacances_VacancesIdVacances",
                table: "Activites",
                column: "VacancesIdVacances",
                principalTable: "Vacances",
                principalColumn: "IdVacances");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activites_Vacances_VacancesIdVacances",
                table: "Activites");

            migrationBuilder.DropIndex(
                name: "IX_Activites_VacancesIdVacances",
                table: "Activites");

            migrationBuilder.DropColumn(
                name: "VacancesIdVacances",
                table: "Activites");
        }
    }
}
