using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MapApplication.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchema3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "ownerShapeId",
                table: "features");

            migrationBuilder.AddColumn<int>(
                name: "ownerShapeIdForPoint",
                table: "features",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ownerShapeIdForWkt",
                table: "features",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_features_ownerShapeIdForPoint",
                table: "features",
                column: "ownerShapeIdForPoint");

            migrationBuilder.CreateIndex(
                name: "IX_features_ownerShapeIdForWkt",
                table: "features",
                column: "ownerShapeIdForWkt");

            migrationBuilder.AddForeignKey(
                name: "FK_Features_Points_OwnerShapeId",
                table: "features",
                column: "ownerShapeIdForPoint",
                principalTable: "points",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Features_Wkt_OwnerShapeId",
                table: "features",
                column: "ownerShapeIdForWkt",
                principalTable: "Wkt",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Features_Points_OwnerShapeId",
                table: "features");

            migrationBuilder.DropForeignKey(
                name: "FK_Features_Wkt_OwnerShapeId",
                table: "features");

            migrationBuilder.DropIndex(
                name: "IX_features_ownerShapeIdForPoint",
                table: "features");

            migrationBuilder.DropIndex(
                name: "IX_features_ownerShapeIdForWkt",
                table: "features");

            migrationBuilder.DropColumn(
                name: "ownerShapeIdForPoint",
                table: "features");

            migrationBuilder.DropColumn(
                name: "ownerShapeIdForWkt",
                table: "features");

            migrationBuilder.AddColumn<int>(
                name: "ownerShapeId",
                table: "features",
                type: "integer",
                nullable: false,
                defaultValue: 0);

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
    }
}
