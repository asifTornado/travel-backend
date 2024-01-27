using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backEnd.Migrations
{
    /// <inheritdoc />
    public partial class addedroomtypetohotelsforbrands3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Rooms",
                table: "Hotels",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rooms",
                table: "Hotels");
        }
    }
}
