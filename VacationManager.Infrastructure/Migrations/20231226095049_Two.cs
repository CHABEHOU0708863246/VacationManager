using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacationManager.Infrastructure.Migrations
{
    public partial class Two : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalUsers",
                table: "VacationsReports",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalUsers",
                table: "VacationsReports");
        }
    }
}
