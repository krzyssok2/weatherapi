using Microsoft.EntityFrameworkCore.Migrations;

namespace WeatherForecastAPI.Migrations
{
    public partial class EnumChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteCities_UserSettings_UserId",
                table: "FavoriteCities");

            migrationBuilder.AlterColumn<int>(
                name: "Units",
                table: "UserSettings",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "FavoriteCities",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Temperature",
                table: "FavoriteCities",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteCities_UserSettings_UserId",
                table: "FavoriteCities",
                column: "UserId",
                principalTable: "UserSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteCities_UserSettings_UserId",
                table: "FavoriteCities");

            migrationBuilder.DropColumn(
                name: "Temperature",
                table: "FavoriteCities");

            migrationBuilder.AlterColumn<string>(
                name: "Units",
                table: "UserSettings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "FavoriteCities",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteCities_UserSettings_UserId",
                table: "FavoriteCities",
                column: "UserId",
                principalTable: "UserSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
