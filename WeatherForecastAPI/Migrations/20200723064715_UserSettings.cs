using Microsoft.EntityFrameworkCore.Migrations;

namespace WeatherForecastAPI.Migrations
{
    public partial class UserSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserSettingsId",
                table: "Cities",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserSettings",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User = table.Column<string>(nullable: true),
                    Units = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSettings", x => x.Id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_UserSettings_UserSettingsId",
                table: "Cities");

            migrationBuilder.DropTable(
                name: "UserSettings");

            migrationBuilder.DropIndex(
                name: "IX_Cities_UserSettingsId",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "UserSettingsId",
                table: "Cities");
        }
    }
}
