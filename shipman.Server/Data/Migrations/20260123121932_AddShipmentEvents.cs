using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shipman.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddShipmentEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShipmentEvent_Shipments_ShipmentId",
                table: "ShipmentEvent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShipmentEvent",
                table: "ShipmentEvent");

            migrationBuilder.RenameTable(
                name: "ShipmentEvent",
                newName: "ShipmentEvents");

            migrationBuilder.RenameIndex(
                name: "IX_ShipmentEvent_ShipmentId",
                table: "ShipmentEvents",
                newName: "IX_ShipmentEvents_ShipmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShipmentEvents",
                table: "ShipmentEvents",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShipmentEvents_Shipments_ShipmentId",
                table: "ShipmentEvents",
                column: "ShipmentId",
                principalTable: "Shipments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShipmentEvents_Shipments_ShipmentId",
                table: "ShipmentEvents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShipmentEvents",
                table: "ShipmentEvents");

            migrationBuilder.RenameTable(
                name: "ShipmentEvents",
                newName: "ShipmentEvent");

            migrationBuilder.RenameIndex(
                name: "IX_ShipmentEvents_ShipmentId",
                table: "ShipmentEvent",
                newName: "IX_ShipmentEvent_ShipmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShipmentEvent",
                table: "ShipmentEvent",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShipmentEvent_Shipments_ShipmentId",
                table: "ShipmentEvent",
                column: "ShipmentId",
                principalTable: "Shipments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
