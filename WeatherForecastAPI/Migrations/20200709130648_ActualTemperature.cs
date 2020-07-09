using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WeatherForecastAPI.Migrations
{
    public partial class ActualTemperature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Forecasts_Cities_CitiesId",
                table: "Forecasts");

            migrationBuilder.AlterColumn<long>(
                name: "CitiesId",
                table: "Forecasts",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ActualTemperatures",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ForecastTime = table.Column<DateTime>(nullable: false),
                    Temperature = table.Column<double>(nullable: false),
                    CitiesId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActualTemperatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActualTemperatures_Cities_CitiesId",
                        column: x => x.CitiesId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActualTemperatures_CitiesId",
                table: "ActualTemperatures",
                column: "CitiesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Forecasts_Cities_CitiesId",
                table: "Forecasts",
                column: "CitiesId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Forecasts_Cities_CitiesId",
                table: "Forecasts");

            migrationBuilder.DropTable(
                name: "ActualTemperatures");

            migrationBuilder.AlterColumn<long>(
                name: "CitiesId",
                table: "Forecasts",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_Forecasts_Cities_CitiesId",
                table: "Forecasts",
                column: "CitiesId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
