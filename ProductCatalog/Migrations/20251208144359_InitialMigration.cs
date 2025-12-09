using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProductCatalog.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Cost = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Cost", "Name" },
                values: new object[,]
                {
                    { new Guid("304e34b1-5267-433a-8d7d-a0abd761da11"), 200, "Шорты" },
                    { new Guid("35e52d12-62f8-4451-ab81-b549fa3f066b"), 50, "Носки" },
                    { new Guid("388ea4e6-f760-4735-9aa5-e3df9906b49c"), 70, "Трусы" },
                    { new Guid("ca173802-206e-4a88-a6cc-2ac93e590fba"), 100, "Футболка" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
