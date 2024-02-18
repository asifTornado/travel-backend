using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backEnd.Migrations
{
    /// <inheritdoc />
    public partial class changedtheexpensereportmodeladdeddisbursement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AmountDisbursed",
                table: "ExpenseReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ExpenseDisbursed",
                table: "ExpenseReports",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountDisbursed",
                table: "ExpenseReports");

            migrationBuilder.DropColumn(
                name: "ExpenseDisbursed",
                table: "ExpenseReports");
        }
    }
}
