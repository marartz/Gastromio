using FoodOrderSystem.Domain.Model.Category;
using FoodOrderSystem.Domain.Model.Shop;
using FoodOrderSystem.Domain.Queries;
using FoodOrderSystem.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodOrderSystem.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<SystemDbContext>();
                    AddTestData(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var queryMediator = services.GetService<IQueryMediator>();
                var result1 = queryMediator.PostAsync<GetAllShopsQuery, ICollection<Shop>>(new GetAllShopsQuery()).Result;
                var shop1 = result1.First();

                var result2 = queryMediator.PostAsync<GetCategoriesOfShopQuery, ICollection<Category>>(new GetCategoriesOfShopQuery(shop1.Id)).Result;
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void AddTestData(SystemDbContext context)
        {
            var paymentMethodCash = new PaymentMethodRow()
            {
                Id = Guid.NewGuid(),
                Name = "Cash",
                Description = "Cash Payment Method"
            };
            context.PaymentMethods.Add(paymentMethodCash);

            var paymentMethodPayPal = new PaymentMethodRow()
            {
                Id = Guid.NewGuid(),
                Name = "Paypal",
                Description = "Paypal Payment Method"
            };
            context.PaymentMethods.Add(paymentMethodPayPal);

            var shop1Id = Guid.NewGuid();
            var shop1 = new ShopRow()
            {
                Id = shop1Id,
                Name = "Test Shop 1",
                AddressLine1 = "Test Street 1",
                AddressLine2 = "",
                AddressZipCode = "12345",
                AddressCity = "Test City",
                DeliveryTimes = new List<DeliveryTimeRow>
                {
                    new DeliveryTimeRow
                    {
                        ShopId = shop1Id,
                        DayOfWeek = 1,
                        StartTime = 720,
                        EndTime = 870
                    }
                },
                MinimumOrderValue = 11.11M,
                DeliveryCosts = 11.12M,
                WebSite = "https://www.testhop1.com",
                Imprint = "Imprint Shop 1",
                ShopPaymentMethods = new List<ShopPaymentMethodRow>
                {
                    new ShopPaymentMethodRow
                    {
                        ShopId = shop1Id,
                        PaymentMethodId = paymentMethodCash.Id
                    }
                }
            };

            shop1.Categories = new List<CategoryRow>
            {
                new CategoryRow
                {
                    Id = Guid.NewGuid(),
                    ShopId = shop1Id,
                    Name = "Cat1",
                }
            };

            var dish1Id = Guid.NewGuid();

            shop1.Dishes = new List<DishRow>
                {
                    new DishRow
                    {
                        Id = dish1Id,
                        ShopId = shop1Id,
                        CategoryId = shop1.Categories.First().Id,
                        Name = "Dish1",
                        Description = "Dish1 Desc",
                        ProductInfo = "Dish1 ProdInfo",
                        Variants = new List<DishVariantRow>()
                        {
                            new DishVariantRow
                            {
                                DishId = dish1Id,
                                Name = "Dish1Var1",
                                Price = 12.34M,
                                Extras = new List<DishVariantExtraRow>
                                {
                                    new DishVariantExtraRow
                                    {
                                        DishId = dish1Id,
                                        VariantName = "Dish1Var1",
                                        Name = "Dish1Var1Extra1",
                                        ProductInfo = "Dish1Var1Extra1 ProdInfo",
                                        Price = 0.5M
                                    }
                                }
                            }
                        }
                    }
                };

            context.Shops.Add(shop1);

            var shop2 = new ShopRow()
            {
                Id = Guid.NewGuid(),
                Name = "Test Shop 2",
                AddressLine1 = "Test Street 2",
                AddressLine2 = "",
                AddressZipCode = "12345",
                AddressCity = "Test City",
                MinimumOrderValue = 22.21M,
                DeliveryCosts = 22.22M,
                WebSite = "https://www.testhop2.com",
                Imprint = "Imprint Shop 2"
            };
            context.Shops.Add(shop2);

            context.SaveChanges();
        }
    }
}
