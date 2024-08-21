using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MapApplication.Migrations
{
    /// <inheritdoc />
    public partial class AddAllTables5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_features_Wkt_ownerId",
                table: "features");

            migrationBuilder.DropForeignKey(
                name: "FK_features_points_ownerId",
                table: "features");

            migrationBuilder.DropIndex(
                name: "IX_features_ownerId",
                table: "features");

            migrationBuilder.CreateIndex(
                name: "IX_features_ownerShapeId",
                table: "features",
                column: "ownerShapeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Features_Wkt_OwnerShapeId",
                table: "features",
                column: "ownerShapeId",
                principalTable: "Wkt",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_features_points_ownerShapeId",
                table: "features",
                column: "ownerShapeId",
                principalTable: "points",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Features_Wkt_OwnerShapeId",
                table: "features");

            migrationBuilder.DropForeignKey(
                name: "FK_features_points_ownerShapeId",
                table: "features");

            migrationBuilder.DropIndex(
                name: "IX_features_ownerShapeId",
                table: "features");

            migrationBuilder.CreateIndex(
                name: "IX_features_ownerId",
                table: "features",
                column: "ownerId");

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
    }
}
