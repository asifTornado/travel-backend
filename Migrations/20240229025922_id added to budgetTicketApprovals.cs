using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backEnd.Migrations
{
    /// <inheritdoc />
    public partial class idaddedtobudgetTicketApprovals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BudgetTicketApprovals",
                table: "BudgetTicketApprovals");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "BudgetTicketApprovals",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BudgetTicketApprovals",
                table: "BudgetTicketApprovals",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetTicketApprovals_BudgetId",
                table: "BudgetTicketApprovals",
                column: "BudgetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BudgetTicketApprovals",
                table: "BudgetTicketApprovals");

            migrationBuilder.DropIndex(
                name: "IX_BudgetTicketApprovals_BudgetId",
                table: "BudgetTicketApprovals");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "BudgetTicketApprovals");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BudgetTicketApprovals",
                table: "BudgetTicketApprovals",
                columns: new[] { "BudgetId", "UserId" });
        }
    }
}
