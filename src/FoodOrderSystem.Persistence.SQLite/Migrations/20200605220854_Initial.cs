using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FoodOrderSystem.Persistence.SQLite.Migrations
{
    public partial class Initial : Migration
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
                name: "PaymentMethod",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethod", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Restaurant",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Image = table.Column<byte[]>(nullable: true),
                    AddressStreet = table.Column<string>(nullable: true),
                    AddressZipCode = table.Column<string>(nullable: true),
                    AddressCity = table.Column<string>(nullable: true),
                    MinimumOrderValue = table.Column<decimal>(type: "decimal(5, 2)", nullable: false),
                    DeliveryCosts = table.Column<decimal>(type: "decimal(5, 2)", nullable: false),
                    Phone = table.Column<string>(nullable: true),
                    WebSite = table.Column<string>(nullable: true),
                    Imprint = table.Column<string>(nullable: true),
                    OrderEmailAddress = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurant", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Role = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PasswordSalt = table.Column<byte[]>(nullable: true),
                    PasswordHash = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryTime",
                columns: table => new
                {
                    RestaurantId = table.Column<Guid>(nullable: false),
                    DayOfWeek = table.Column<int>(nullable: false),
                    StartTime = table.Column<int>(nullable: false),
                    EndTime = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryTime", x => new { x.RestaurantId, x.DayOfWeek, x.StartTime });
                    table.ForeignKey(
                        name: "FK_DeliveryTime_Restaurant_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DishCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RestaurantId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    OrderNo = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DishCategory_Restaurant_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RestaurantCuisine",
                columns: table => new
                {
                    RestaurantId = table.Column<Guid>(nullable: false),
                    CuisineId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantCuisine", x => new { x.RestaurantId, x.CuisineId });
                    table.ForeignKey(
                        name: "FK_RestaurantCuisine_Cuisine_CuisineId",
                        column: x => x.CuisineId,
                        principalTable: "Cuisine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RestaurantCuisine_Restaurant_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RestaurantPaymentMethod",
                columns: table => new
                {
                    RestaurantId = table.Column<Guid>(nullable: false),
                    PaymentMethodId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantPaymentMethod", x => new { x.RestaurantId, x.PaymentMethodId });
                    table.ForeignKey(
                        name: "FK_RestaurantPaymentMethod_PaymentMethod_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "PaymentMethod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RestaurantPaymentMethod_Restaurant_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RestaurantUser",
                columns: table => new
                {
                    RestaurantId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantUser", x => new { x.RestaurantId, x.UserId });
                    table.ForeignKey(
                        name: "FK_RestaurantUser_Restaurant_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RestaurantUser_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dish",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RestaurantId = table.Column<Guid>(nullable: false),
                    CategoryId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ProductInfo = table.Column<string>(nullable: true),
                    OrderNo = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dish", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dish_DishCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "DishCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dish_Restaurant_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DishVariant",
                columns: table => new
                {
                    DishId = table.Column<Guid>(nullable: false),
                    VariantId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(5, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishVariant", x => new { x.DishId, x.VariantId });
                    table.ForeignKey(
                        name: "FK_DishVariant_Dish_DishId",
                        column: x => x.DishId,
                        principalTable: "Dish",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DishVariantExtra",
                columns: table => new
                {
                    DishId = table.Column<Guid>(nullable: false),
                    VariantId = table.Column<Guid>(nullable: false),
                    ExtraId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", nullable: true),
                    ProductInfo = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(type: "decimal(5, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishVariantExtra", x => new { x.DishId, x.VariantId, x.ExtraId });
                    table.ForeignKey(
                        name: "FK_DishVariantExtra_Dish_DishId",
                        column: x => x.DishId,
                        principalTable: "Dish",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DishVariantExtra_DishVariant_DishId_VariantId",
                        columns: x => new { x.DishId, x.VariantId },
                        principalTable: "DishVariant",
                        principalColumns: new[] { "DishId", "VariantId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dish_CategoryId",
                table: "Dish",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Dish_RestaurantId",
                table: "Dish",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_DishCategory_RestaurantId",
                table: "DishCategory",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantCuisine_CuisineId",
                table: "RestaurantCuisine",
                column: "CuisineId");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantPaymentMethod_PaymentMethodId",
                table: "RestaurantPaymentMethod",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantUser_UserId",
                table: "RestaurantUser",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryTime");

            migrationBuilder.DropTable(
                name: "DishVariantExtra");

            migrationBuilder.DropTable(
                name: "RestaurantCuisine");

            migrationBuilder.DropTable(
                name: "RestaurantPaymentMethod");

            migrationBuilder.DropTable(
                name: "RestaurantUser");

            migrationBuilder.DropTable(
                name: "DishVariant");

            migrationBuilder.DropTable(
                name: "Cuisine");

            migrationBuilder.DropTable(
                name: "PaymentMethod");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Dish");

            migrationBuilder.DropTable(
                name: "DishCategory");

            migrationBuilder.DropTable(
                name: "Restaurant");
        }
    }
}
