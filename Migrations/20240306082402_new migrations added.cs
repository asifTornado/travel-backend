using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backEnd.Migrations
{
    /// <inheritdoc />
    public partial class newmigrationsadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountsPrevHandlerIds",
                table: "Budgets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AccountsProcessed",
                table: "Budgets",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuditPrevHandlerIds",
                table: "Budgets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AuditProcessed",
                table: "Budgets",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "BeingProcessed",
                table: "Budgets",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "BeingProcessedAccounts",
                table: "Budgets",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "BeingProcessedAudit",
                table: "Budgets",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrentAccountsHandlerId",
                table: "Budgets",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrentAuditHandlerId",
                table: "Budgets",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountsPrevHandlerIds",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "AccountsProcessed",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "AuditPrevHandlerIds",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "AuditProcessed",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "BeingProcessed",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "BeingProcessedAccounts",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "BeingProcessedAudit",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "CurrentAccountsHandlerId",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "CurrentAuditHandlerId",
                table: "Budgets");
        }
    }
}
