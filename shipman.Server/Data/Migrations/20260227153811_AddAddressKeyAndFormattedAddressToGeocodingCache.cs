using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shipman.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAddressKeyAndFormattedAddressToGeocodingCache : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Address",
                table: "GeocodingCache",
                newName: "FormattedAddress");

            migrationBuilder.AddColumn<string>(
                name: "AddressKey",
                table: "GeocodingCache",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressKey",
                table: "GeocodingCache");

            migrationBuilder.RenameColumn(
                name: "FormattedAddress",
                table: "GeocodingCache",
                newName: "Address");
        }
    }
}
