using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MapApplication.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchema4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "features");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "features",
                columns: table => new
                {
                    FeatureId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    featureData = table.Column<string>(type: "text", nullable: false),
                    featureName = table.Column<string>(type: "text", nullable: false),
                    ownerId = table.Column<int>(type: "integer", nullable: false),
                    ownerShapeIdForPoint = table.Column<int>(type: "integer", nullable: true),
                    ownerShapeIdForWkt = table.Column<int>(type: "integer", nullable: true),
                    ownerShapeType = table.Column<string>(type: "text", nullable: false),
                    createdDate = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_features", x => x.FeatureId);
                    table.ForeignKey(
                        name: "FK_Features_Points_OwnerShapeId",
                        column: x => x.ownerShapeIdForPoint,
                        principalTable: "points",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Features_Wkt_OwnerShapeId",
                        column: x => x.ownerShapeIdForWkt,
                        principalTable: "Wkt",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_features_ownerShapeIdForPoint",
                table: "features",
                column: "ownerShapeIdForPoint");

            migrationBuilder.CreateIndex(
                name: "IX_features_ownerShapeIdForWkt",
                table: "features",
                column: "ownerShapeIdForWkt");
        }
    }
}
