using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backEnd.Migrations
{
    /// <inheritdoc />
    public partial class migrationsaddedfornotificationmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Requests_TicketId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_TicketId",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "TicketId",
                table: "Notifications",
                newName: "SourceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SourceId",
                table: "Notifications",
                newName: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_TicketId",
                table: "Notifications",
                column: "TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Requests_TicketId",
                table: "Notifications",
                column: "TicketId",
                principalTable: "Requests",
                principalColumn: "Id");
        }
    }
}
