using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WheelOfFortune.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWheelOfFortune : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RemainingOptions",
                table: "PointWheels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RemainingOptions",
                table: "ClassicWheels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RemainingOptions",
                table: "PointWheels");

            migrationBuilder.DropColumn(
                name: "RemainingOptions",
                table: "ClassicWheels");
        }
    }
}
