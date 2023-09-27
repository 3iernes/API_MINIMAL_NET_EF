using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_ESP_GW.Migrations
{
    /// <inheritdoc />
    public partial class PosParaStepper : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Pos",
                table: "BarCodes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pos",
                table: "BarCodes");
        }
    }
}
