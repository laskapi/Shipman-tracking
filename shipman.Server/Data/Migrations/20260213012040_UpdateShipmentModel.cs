using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shipman.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateShipmentModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReceiverPhone",
                table: "Shipments",
                newName: "Sender_Phone");

            migrationBuilder.RenameColumn(
                name: "ReceiverName",
                table: "Shipments",
                newName: "Sender_Name");

            migrationBuilder.RenameColumn(
                name: "ReceiverEmail",
                table: "Shipments",
                newName: "Sender_Email");

            migrationBuilder.RenameColumn(
                name: "Sender",
                table: "Shipments",
                newName: "Receiver_Phone");

            migrationBuilder.AddColumn<double>(
                name: "DestinationCoordinates_Lat",
                table: "Shipments",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DestinationCoordinates_Lng",
                table: "Shipments",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "OriginCoordinates_Lat",
                table: "Shipments",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "OriginCoordinates_Lng",
                table: "Shipments",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Receiver_Email",
                table: "Shipments",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Receiver_Name",
                table: "Shipments",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DestinationCoordinates_Lat",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "DestinationCoordinates_Lng",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "OriginCoordinates_Lat",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "OriginCoordinates_Lng",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Receiver_Email",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Receiver_Name",
                table: "Shipments");

            migrationBuilder.RenameColumn(
                name: "Sender_Phone",
                table: "Shipments",
                newName: "ReceiverPhone");

            migrationBuilder.RenameColumn(
                name: "Sender_Name",
                table: "Shipments",
                newName: "ReceiverName");

            migrationBuilder.RenameColumn(
                name: "Sender_Email",
                table: "Shipments",
                newName: "ReceiverEmail");

            migrationBuilder.RenameColumn(
                name: "Receiver_Phone",
                table: "Shipments",
                newName: "Sender");
        }
    }
}
