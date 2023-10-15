using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacancyProAPI.Migrations
{
    /// <inheritdoc />
    public partial class init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activites_Vacances_VacancesIdVacances",
                table: "Activites");

            migrationBuilder.DropIndex(
                name: "IX_Activites_VacancesIdVacances",
                table: "Activites");

            migrationBuilder.DropColumn(
                name: "DateDebut",
                table: "Vacances");

            migrationBuilder.DropColumn(
                name: "DateFin",
                table: "Vacances");

            migrationBuilder.DropColumn(
                name: "JourDebut",
                table: "Activites");

            migrationBuilder.DropColumn(
                name: "JourFin",
                table: "Activites");

            migrationBuilder.DropColumn(
                name: "VacancesIdVacances",
                table: "Activites");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateDebut",
                table: "Vacances",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateFin",
                table: "Vacances",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "JourDebut",
                table: "Activites",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "JourFin",
                table: "Activites",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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
    }
}
