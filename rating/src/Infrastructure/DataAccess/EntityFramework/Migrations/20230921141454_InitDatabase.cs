using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.DataAccess.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class InitDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "ratings",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    customer_id = table.Column<uint>(type: "oid", nullable: false),
                    craftsman_id = table.Column<uint>(type: "oid", nullable: false),
                    task_id = table.Column<uint>(type: "oid", nullable: false),
                    score = table.Column<uint>(type: "oid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ratings", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ratings_id_craftsman_id",
                schema: "public",
                table: "ratings",
                columns: new[] { "id", "craftsman_id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ratings",
                schema: "public");
        }
    }
}
