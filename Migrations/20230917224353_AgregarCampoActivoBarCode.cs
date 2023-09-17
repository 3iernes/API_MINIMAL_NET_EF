using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_ESP_GW.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCampoActivoBarCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "BarCodes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "BarCodes");
        }
    }
}
