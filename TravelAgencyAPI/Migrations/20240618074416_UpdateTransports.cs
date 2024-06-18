using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelAgencyAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTransports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceForHundredKm",
                table: "Transports");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "PriceForHundredKm",
                table: "Transports",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
