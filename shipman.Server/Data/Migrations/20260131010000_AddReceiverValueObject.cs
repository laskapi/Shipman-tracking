using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shipman.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddReceiverValueObject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Receiver",
                table: "Shipments",
                newName: "ReceiverPhone");

            migrationBuilder.AddColumn<string>(
                name: "ReceiverEmail",
                table: "Shipments",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReceiverName",
                table: "Shipments",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiverEmail",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "ReceiverName",
                table: "Shipments");

            migrationBuilder.RenameColumn(
                name: "ReceiverPhone",
                table: "Shipments",
                newName: "Receiver");
        }
    }
}
