using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backEnd.Migrations
{
    /// <inheritdoc />
    public partial class moneyreceiptchanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PrevHandlerId",
                table: "MoneyReceipts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Submitted",
                table: "MoneyReceipts",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SupervisorApproved",
                table: "MoneyReceipts",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrevHandlerId",
                table: "MoneyReceipts");

            migrationBuilder.DropColumn(
                name: "Submitted",
                table: "MoneyReceipts");

            migrationBuilder.DropColumn(
                name: "SupervisorApproved",
                table: "MoneyReceipts");
        }
    }
}
