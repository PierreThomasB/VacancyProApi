using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacancyProAPI.Migrations
{
    /// <inheritdoc />
    public partial class lieuToActivite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LieuxIdLieu",
                table: "Activites",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Activites_LieuxIdLieu",
                table: "Activites",
                column: "LieuxIdLieu");

            migrationBuilder.AddForeignKey(
                name: "FK_Activites_Lieux_LieuxIdLieu",
                table: "Activites",
                column: "LieuxIdLieu",
                principalTable: "Lieux",
                principalColumn: "IdLieu",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activites_Lieux_LieuxIdLieu",
                table: "Activites");

            migrationBuilder.DropIndex(
                name: "IX_Activites_LieuxIdLieu",
                table: "Activites");

            migrationBuilder.DropColumn(
                name: "LieuxIdLieu",
                table: "Activites");
        }
    }
}
