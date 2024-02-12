using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backEnd.Migrations
{
    /// <inheritdoc />
    public partial class changedmoneyreceipt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AsAdvaneAgainst",
                table: "MoneyReceipts",
                newName: "Taka");

            migrationBuilder.AddColumn<string>(
                name: "AsAdvanceAgainst",
                table: "MoneyReceipts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AsAdvanceAgainst",
                table: "MoneyReceipts");

            migrationBuilder.RenameColumn(
                name: "Taka",
                table: "MoneyReceipts",
                newName: "AsAdvaneAgainst");
        }
    }
}
