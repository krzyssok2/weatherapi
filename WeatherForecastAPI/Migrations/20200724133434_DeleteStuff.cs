using Microsoft.EntityFrameworkCore.Migrations;

namespace WeatherForecastAPI.Migrations
{
    public partial class DeleteStuff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Temperature",
                table: "FavoriteCities");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Temperature",
                table: "FavoriteCities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
