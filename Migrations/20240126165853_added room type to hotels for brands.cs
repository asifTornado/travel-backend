using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backEnd.Migrations
{
    /// <inheritdoc />
    public partial class addedroomtypetohotelsforbrands : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Actual_rate",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "Average_rate",
                table: "Hotels");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Actual_rate",
                table: "Hotels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Average_rate",
                table: "Hotels",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
