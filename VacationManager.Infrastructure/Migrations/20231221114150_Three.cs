using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacationManager.Infrastructure.Migrations
{
    public partial class Three : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RejectedRequests",
                table: "VacationsReports",
                newName: "TotalRejected");

            migrationBuilder.RenameColumn(
                name: "PendingRequests",
                table: "VacationsReports",
                newName: "TotalPending");

            migrationBuilder.RenameColumn(
                name: "ApprovedRequests",
                table: "VacationsReports",
                newName: "TotalDemand");

            migrationBuilder.AddColumn<int>(
                name: "TotalApproved",
                table: "VacationsReports",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalApproved",
                table: "VacationsReports");

            migrationBuilder.RenameColumn(
                name: "TotalRejected",
                table: "VacationsReports",
                newName: "RejectedRequests");

            migrationBuilder.RenameColumn(
                name: "TotalPending",
                table: "VacationsReports",
                newName: "PendingRequests");

            migrationBuilder.RenameColumn(
                name: "TotalDemand",
                table: "VacationsReports",
                newName: "ApprovedRequests");
        }
    }
}
