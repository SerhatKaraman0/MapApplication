using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MapApplication.Migrations
{
    /// <inheritdoc />
    public partial class AddAllTables4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_features_Wkt_OwnerId",
                table: "features");

            migrationBuilder.DropForeignKey(
                name: "FK_features_points_OwnerId",
                table: "features");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "features",
                newName: "ownerId");

            migrationBuilder.RenameIndex(
                name: "IX_features_OwnerId",
                table: "features",
                newName: "IX_features_ownerId");

            migrationBuilder.AddColumn<int>(
                name: "ownerShapeId",
                table: "features",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ownerShapeType",
                table: "features",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_features_Wkt_ownerId",
                table: "features",
                column: "ownerId",
                principalTable: "Wkt",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_features_points_ownerId",
                table: "features",
                column: "ownerId",
                principalTable: "points",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_features_Wkt_ownerId",
                table: "features");

            migrationBuilder.DropForeignKey(
                name: "FK_features_points_ownerId",
                table: "features");

            migrationBuilder.DropColumn(
                name: "ownerShapeId",
                table: "features");

            migrationBuilder.DropColumn(
                name: "ownerShapeType",
                table: "features");

            migrationBuilder.RenameColumn(
                name: "ownerId",
                table: "features",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_features_ownerId",
                table: "features",
                newName: "IX_features_OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_features_Wkt_OwnerId",
                table: "features",
                column: "OwnerId",
                principalTable: "Wkt",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_features_points_OwnerId",
                table: "features",
                column: "OwnerId",
                principalTable: "points",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
