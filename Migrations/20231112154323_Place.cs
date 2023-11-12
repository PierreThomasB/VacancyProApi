using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacancyProAPI.Migrations
{
    /// <inheritdoc />
    public partial class Place : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Place",
                table: "Periods");

            migrationBuilder.AddColumn<string>(
                name: "PlaceId",
                table: "Periods",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Places",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UrlPhoto = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Places", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Periods_PlaceId",
                table: "Periods",
                column: "PlaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Periods_Places_PlaceId",
                table: "Periods",
                column: "PlaceId",
                principalTable: "Places",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Periods_Places_PlaceId",
                table: "Periods");

            migrationBuilder.DropTable(
                name: "Places");

            migrationBuilder.DropIndex(
                name: "IX_Periods_PlaceId",
                table: "Periods");

            migrationBuilder.DropColumn(
                name: "PlaceId",
                table: "Periods");

            migrationBuilder.AddColumn<string>(
                name: "Place",
                table: "Periods",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
