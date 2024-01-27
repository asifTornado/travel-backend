using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backEnd.Migrations
{
    /// <inheritdoc />
    public partial class addedtotalcoststoquotations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TotalCosts",
                table: "Quotations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TotalCosts",
                table: "HotelQuotations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalCosts",
                table: "Quotations");

            migrationBuilder.DropColumn(
                name: "TotalCosts",
                table: "HotelQuotations");
        }
    }
}
