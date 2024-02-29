using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backEnd.Migrations
{
    /// <inheritdoc />
    public partial class anothermigrationsaddedpleasework : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentHandlerId",
                table: "Budgets",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrevHandlerIds",
                table: "Budgets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Processed",
                table: "Budgets",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Rejected",
                table: "Budgets",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SeekingAccountsApprovalForTickets",
                table: "Budgets",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TicketsApprovedByAccounts",
                table: "Budgets",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BudgetTicketApprovals",
                columns: table => new
                {
                    BudgetId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetTicketApprovals", x => new { x.BudgetId, x.UserId });
                    table.ForeignKey(
                        name: "FK_BudgetTicketApprovals_Budgets_BudgetId",
                        column: x => x.BudgetId,
                        principalTable: "Budgets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BudgetTicketApprovals_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BudgetTicketApprovals_UserId",
                table: "BudgetTicketApprovals",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetTicketApprovals");

            migrationBuilder.DropColumn(
                name: "CurrentHandlerId",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "PrevHandlerIds",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "Processed",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "Rejected",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "SeekingAccountsApprovalForTickets",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "TicketsApprovedByAccounts",
                table: "Budgets");
        }
    }
}
