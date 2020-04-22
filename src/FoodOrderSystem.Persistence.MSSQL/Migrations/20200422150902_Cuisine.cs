using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FoodOrderSystem.Persistence.MSSQL.Migrations
{
    public partial class Cuisine : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cuisine",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cuisine", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RestaurantCuisineRow",
                columns: table => new
                {
                    RestaurantId = table.Column<Guid>(nullable: false),
                    CuisineId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantCuisineRow", x => new { x.RestaurantId, x.CuisineId });
                    table.ForeignKey(
                        name: "FK_RestaurantCuisineRow_Cuisine_CuisineId",
                        column: x => x.CuisineId,
                        principalTable: "Cuisine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RestaurantCuisineRow_Restaurant_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantCuisineRow_CuisineId",
                table: "RestaurantCuisineRow",
                column: "CuisineId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RestaurantCuisineRow");

            migrationBuilder.DropTable(
                name: "Cuisine");
        }
    }
}
