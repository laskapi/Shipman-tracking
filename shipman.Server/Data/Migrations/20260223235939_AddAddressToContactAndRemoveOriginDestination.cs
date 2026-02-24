using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shipman.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAddressToContactAndRemoveOriginDestination : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Origin",
                table: "Shipments",
                newName: "Sender_Address");

            migrationBuilder.RenameColumn(
                name: "Destination",
                table: "Shipments",
                newName: "Receiver_Address");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Sender_Address",
                table: "Shipments",
                newName: "Origin");

            migrationBuilder.RenameColumn(
                name: "Receiver_Address",
                table: "Shipments",
                newName: "Destination");
        }
    }
}
