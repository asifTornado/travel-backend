using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backEnd.Migrations
{
    /// <inheritdoc />
    public partial class addedlistofprevhandlers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrevHandlerId",
                table: "MoneyReceipts");

            migrationBuilder.DropColumn(
                name: "PrevHandlerId",
                table: "ExpenseReports");

            migrationBuilder.AddColumn<string>(
                name: "PrevHandlerIds",
                table: "MoneyReceipts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrevHandlerIds",
                table: "ExpenseReports",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrevHandlerIds",
                table: "MoneyReceipts");

            migrationBuilder.DropColumn(
                name: "PrevHandlerIds",
                table: "ExpenseReports");

            migrationBuilder.AddColumn<int>(
                name: "PrevHandlerId",
                table: "MoneyReceipts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PrevHandlerId",
                table: "ExpenseReports",
                type: "int",
                nullable: true);
        }
    }
}
