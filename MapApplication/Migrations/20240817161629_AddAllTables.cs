using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MapApplication.Migrations
{
    /// <inheritdoc />
    public partial class AddAllTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Y_coordinate",
                table: "points",
                newName: "y_coordinate");

            migrationBuilder.RenameColumn(
                name: "X_coordinate",
                table: "points",
                newName: "x_coordinate");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "points",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "points",
                newName: "date");

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Wkt",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "owner_id",
                table: "points",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "features",
                columns: table => new
                {
                    FeatureId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OwnerId = table.Column<int>(type: "integer", nullable: false),
                    featureName = table.Column<string>(type: "text", nullable: false),
                    featureData = table.Column<string>(type: "text", nullable: false),
                    createdDate = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_features", x => x.FeatureId);
                    table.ForeignKey(
                        name: "FK_features_Wkt_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Wkt",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_features_points_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "points",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userName = table.Column<string>(type: "text", nullable: false),
                    userEmail = table.Column<string>(type: "text", nullable: false),
                    userPassword = table.Column<string>(type: "text", nullable: false),
                    createdDate = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "tabs",
                columns: table => new
                {
                    TabId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tabName = table.Column<string>(type: "text", nullable: false),
                    tabColor = table.Column<string>(type: "text", nullable: false),
                    OwnerId = table.Column<int>(type: "integer", nullable: false),
                    createdDate = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tabs", x => x.TabId);
                    table.ForeignKey(
                        name: "FK_tabs_users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wkt_OwnerId",
                table: "Wkt",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_points_owner_id",
                table: "points",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_features_OwnerId",
                table: "features",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_tabs_OwnerId",
                table: "tabs",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_points_users_owner_id",
                table: "points",
                column: "owner_id",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wkt_users_OwnerId",
                table: "Wkt",
                column: "OwnerId",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_points_users_owner_id",
                table: "points");

            migrationBuilder.DropForeignKey(
                name: "FK_Wkt_users_OwnerId",
                table: "Wkt");

            migrationBuilder.DropTable(
                name: "features");

            migrationBuilder.DropTable(
                name: "tabs");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropIndex(
                name: "IX_Wkt_OwnerId",
                table: "Wkt");

            migrationBuilder.DropIndex(
                name: "IX_points_owner_id",
                table: "points");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Wkt");

            migrationBuilder.DropColumn(
                name: "owner_id",
                table: "points");

            migrationBuilder.RenameColumn(
                name: "y_coordinate",
                table: "points",
                newName: "Y_coordinate");

            migrationBuilder.RenameColumn(
                name: "x_coordinate",
                table: "points",
                newName: "X_coordinate");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "points",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "points",
                newName: "Date");
        }
    }
}
