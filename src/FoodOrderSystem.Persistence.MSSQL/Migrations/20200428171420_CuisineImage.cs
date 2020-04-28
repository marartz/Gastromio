using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FoodOrderSystem.Persistence.MSSQL.Migrations
{
    public partial class CuisineImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Cuisine",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Cuisine");
        }
    }
}
