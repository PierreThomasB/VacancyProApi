using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacancyProAPI.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnonymUsers",
                columns: table => new
                {
                    IdAnonym = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sujet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsResolve = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnonymUsers", x => x.IdAnonym);
                });

            migrationBuilder.CreateTable(
                name: "Lieux",
                columns: table => new
                {
                    IdLieu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Lieu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lieux", x => x.IdLieu);
                });

            migrationBuilder.CreateTable(
                name: "Vacances",
                columns: table => new
                {
                    IdVacances = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateDebut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LieuxIdLieu = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vacances", x => x.IdVacances);
                    table.ForeignKey(
                        name: "FK_Vacances_Lieux_LieuxIdLieu",
                        column: x => x.LieuxIdLieu,
                        principalTable: "Lieux",
                        principalColumn: "IdLieu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Activites",
                columns: table => new
                {
                    IdActivite = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LieuxIdLieu = table.Column<int>(type: "int", nullable: false),
                    VacancesIdVacances = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activites", x => x.IdActivite);
                    table.ForeignKey(
                        name: "FK_Activites_Lieux_LieuxIdLieu",
                        column: x => x.LieuxIdLieu,
                        principalTable: "Lieux",
                        principalColumn: "IdLieu",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Activites_Vacances_VacancesIdVacances",
                        column: x => x.VacancesIdVacances,
                        principalTable: "Vacances",
                        principalColumn: "IdVacances");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activites_LieuxIdLieu",
                table: "Activites",
                column: "LieuxIdLieu");

            migrationBuilder.CreateIndex(
                name: "IX_Activites_VacancesIdVacances",
                table: "Activites",
                column: "VacancesIdVacances");

            migrationBuilder.CreateIndex(
                name: "IX_Vacances_LieuxIdLieu",
                table: "Vacances",
                column: "LieuxIdLieu");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activites");

            migrationBuilder.DropTable(
                name: "AnonymUsers");

            migrationBuilder.DropTable(
                name: "Vacances");

            migrationBuilder.DropTable(
                name: "Lieux");
        }
    }
}
