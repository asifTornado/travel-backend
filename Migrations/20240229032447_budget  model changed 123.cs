using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backEnd.Migrations
{
    /// <inheritdoc />
    public partial class budgetmodelchanged123 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AmountDisbursedTickets",
                table: "Budgets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TicketsAccountHolderNumber",
                table: "Budgets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TicketsAccountNumber",
                table: "Budgets",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountDisbursedTickets",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "TicketsAccountHolderNumber",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "TicketsAccountNumber",
                table: "Budgets");
        }
    }
}
