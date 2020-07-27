using Microsoft.EntityFrameworkCore.Migrations;

namespace WeatherForecastAPI.Migrations
{
    public partial class FavoriteCitiesFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_UserSettings_UserSettingsId",
                table: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Cities_UserSettingsId",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "UserSettingsId",
                table: "Cities");

            migrationBuilder.CreateTable(
                name: "FavoriteCities",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(nullable: true),
                    CityId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteCities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoriteCities_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FavoriteCities_UserSettings_UserId",
                        column: x => x.UserId,
                        principalTable: "UserSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteCities_CityId",
                table: "FavoriteCities",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteCities_UserId",
                table: "FavoriteCities",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoriteCities");

            migrationBuilder.AddColumn<long>(
                name: "UserSettingsId",
                table: "Cities",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cities_UserSettingsId",
                table: "Cities",
                column: "UserSettingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_UserSettings_UserSettingsId",
                table: "Cities",
                column: "UserSettingsId",
                principalTable: "UserSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
