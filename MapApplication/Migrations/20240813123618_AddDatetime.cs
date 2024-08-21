using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MapApplication.Migrations
{
    /// <inheritdoc />
    public partial class AddDatetime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Date",
                table: "Wkt",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Date",
                table: "points",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Wkt");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "points");
        }
    }
}
