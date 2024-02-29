using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backEnd.Migrations
{
    /// <inheritdoc />
    public partial class changedthemoneyreceiptandexpensereportmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExpenseDisbursed",
                table: "ExpenseReports",
                newName: "Disbursed");

            migrationBuilder.AddColumn<string>(
                name: "AmountDisbursed",
                table: "MoneyReceipts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankAccountHolderName",
                table: "MoneyReceipts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankAccountNumber",
                table: "MoneyReceipts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Disbursed",
                table: "MoneyReceipts",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankAccountHolderName",
                table: "ExpenseReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankAccountNumber",
                table: "ExpenseReports",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountDisbursed",
                table: "MoneyReceipts");

            migrationBuilder.DropColumn(
                name: "BankAccountHolderName",
                table: "MoneyReceipts");

            migrationBuilder.DropColumn(
                name: "BankAccountNumber",
                table: "MoneyReceipts");

            migrationBuilder.DropColumn(
                name: "Disbursed",
                table: "MoneyReceipts");

            migrationBuilder.DropColumn(
                name: "BankAccountHolderName",
                table: "ExpenseReports");

            migrationBuilder.DropColumn(
                name: "BankAccountNumber",
                table: "ExpenseReports");

            migrationBuilder.RenameColumn(
                name: "Disbursed",
                table: "ExpenseReports",
                newName: "ExpenseDisbursed");
        }
    }
}
