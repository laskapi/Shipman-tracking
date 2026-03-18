using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shipman.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeContactFieldsRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "Receiver_Address",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Receiver_Email",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Receiver_Name",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Receiver_Phone",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Sender_Address",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "GeocodingCache");

            migrationBuilder.RenameColumn(
                name: "Sender_Phone",
                table: "Shipments",
                newName: "SenderId");

            migrationBuilder.RenameColumn(
                name: "Sender_Name",
                table: "Shipments",
                newName: "ReceiverId");

            migrationBuilder.RenameColumn(
                name: "Sender_Email",
                table: "Shipments",
                newName: "DestinationAddressId");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "ShipmentEvents",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ShipmentEvents",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Street = table.Column<string>(type: "TEXT", nullable: false),
                    HouseNumber = table.Column<string>(type: "TEXT", nullable: false),
                    ApartmentNumber = table.Column<string>(type: "TEXT", nullable: true),
                    City = table.Column<string>(type: "TEXT", nullable: false),
                    PostalCode = table.Column<string>(type: "TEXT", nullable: false),
                    Country = table.Column<string>(type: "TEXT", nullable: false),
                    Latitude = table.Column<double>(type: "REAL", nullable: false),
                    Longitude = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: false),
                    PrimaryAddressId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contacts_Addresses_PrimaryAddressId",
                        column: x => x.PrimaryAddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContactDestinationAddresses",
                columns: table => new
                {
                    ContactId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AddressId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactDestinationAddresses", x => new { x.ContactId, x.AddressId });
                    table.ForeignKey(
                        name: "FK_ContactDestinationAddresses_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContactDestinationAddresses_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_DestinationAddressId",
                table: "Shipments",
                column: "DestinationAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_ReceiverId",
                table: "Shipments",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_SenderId",
                table: "Shipments",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactDestinationAddresses_AddressId",
                table: "ContactDestinationAddresses",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_PrimaryAddressId",
                table: "Contacts",
                column: "PrimaryAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shipments_Addresses_DestinationAddressId",
                table: "Shipments",
                column: "DestinationAddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Shipments_Contacts_ReceiverId",
                table: "Shipments",
                column: "ReceiverId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Shipments_Contacts_SenderId",
                table: "Shipments",
                column: "SenderId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shipments_Addresses_DestinationAddressId",
                table: "Shipments");

            migrationBuilder.DropForeignKey(
                name: "FK_Shipments_Contacts_ReceiverId",
                table: "Shipments");

            migrationBuilder.DropForeignKey(
                name: "FK_Shipments_Contacts_SenderId",
                table: "Shipments");

            migrationBuilder.DropTable(
                name: "ContactDestinationAddresses");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Shipments_DestinationAddressId",
                table: "Shipments");

            migrationBuilder.DropIndex(
                name: "IX_Shipments_ReceiverId",
                table: "Shipments");

            migrationBuilder.DropIndex(
                name: "IX_Shipments_SenderId",
                table: "Shipments");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "Shipments",
                newName: "Sender_Phone");

            migrationBuilder.RenameColumn(
                name: "ReceiverId",
                table: "Shipments",
                newName: "Sender_Name");

            migrationBuilder.RenameColumn(
                name: "DestinationAddressId",
                table: "Shipments",
                newName: "Sender_Email");

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
                name: "Receiver_Address",
                table: "Shipments",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

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

            migrationBuilder.AddColumn<string>(
                name: "Receiver_Phone",
                table: "Shipments",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Sender_Address",
                table: "Shipments",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "ShipmentEvents",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ShipmentEvents",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "GeocodingCache",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
