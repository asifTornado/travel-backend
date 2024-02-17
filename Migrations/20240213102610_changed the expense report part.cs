using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backEnd.Migrations
{
    /// <inheritdoc />
    public partial class changedtheexpensereportpart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Approvals",
                table: "ExpenseReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrentHandlerId",
                table: "ExpenseReports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PrevHandlerId",
                table: "ExpenseReports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Processed",
                table: "ExpenseReports",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "ExpenseReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Submitted",
                table: "ExpenseReports",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SupervisorApproved",
                table: "ExpenseReports",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseReports_CurrentHandlerId",
                table: "ExpenseReports",
                column: "CurrentHandlerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseReports_Users_CurrentHandlerId",
                table: "ExpenseReports",
                column: "CurrentHandlerId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseReports_Users_CurrentHandlerId",
                table: "ExpenseReports");

            migrationBuilder.DropIndex(
                name: "IX_ExpenseReports_CurrentHandlerId",
                table: "ExpenseReports");

            migrationBuilder.DropColumn(
                name: "Approvals",
                table: "ExpenseReports");

            migrationBuilder.DropColumn(
                name: "CurrentHandlerId",
                table: "ExpenseReports");

            migrationBuilder.DropColumn(
                name: "PrevHandlerId",
                table: "ExpenseReports");

            migrationBuilder.DropColumn(
                name: "Processed",
                table: "ExpenseReports");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ExpenseReports");

            migrationBuilder.DropColumn(
                name: "Submitted",
                table: "ExpenseReports");

            migrationBuilder.DropColumn(
                name: "SupervisorApproved",
                table: "ExpenseReports");
        }
    }
}
