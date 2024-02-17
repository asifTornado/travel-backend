using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backEnd.Migrations
{
    /// <inheritdoc />
    public partial class addedtravelmanagersubmitttedtoexpensereport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Rejected",
                table: "MoneyReceipts",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Rejected",
                table: "ExpenseReports",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TravelManagerSubmitted",
                table: "ExpenseReports",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rejected",
                table: "MoneyReceipts");

            migrationBuilder.DropColumn(
                name: "Rejected",
                table: "ExpenseReports");

            migrationBuilder.DropColumn(
                name: "TravelManagerSubmitted",
                table: "ExpenseReports");
        }
    }
}
