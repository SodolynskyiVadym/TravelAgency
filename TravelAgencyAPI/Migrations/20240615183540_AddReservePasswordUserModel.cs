using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelAgencyAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddReservePasswordUserModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ReservePasswordHash",
                table: "Users",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ReservePasswordSalt",
                table: "Users",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReservePasswordHash",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ReservePasswordSalt",
                table: "Users");
        }
    }
}
