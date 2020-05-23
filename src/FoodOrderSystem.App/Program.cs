using FoodOrderSystem.Domain.Commands;
using FoodOrderSystem.Domain.Commands.EnsureAdminUser;
using FoodOrderSystem.Domain.Model.User;
using FoodOrderSystem.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using FoodOrderSystem.Domain.Commands.AddTestData;

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
                var dbContext = services.GetService<SystemDbContext>();
                dbContext.Database.Migrate();
            }

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var currentUser = new User(new UserId(Guid.Empty), "admin", Role.SystemAdmin, null, null, null);

                var commandDispatcher = services.GetService<ICommandDispatcher>();
                var result = commandDispatcher.PostAsync<EnsureAdminUserCommand, bool>(new EnsureAdminUserCommand(), currentUser).Result;
            }

            if (args.Length == 1 && args[0] == @"testdata")
            {
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;

                    var currentUser = new User(new UserId(Guid.Empty), "admin", Role.SystemAdmin, null, null, null);

                    var commandDispatcher = services.GetService<ICommandDispatcher>();
                    var result = commandDispatcher
                        .PostAsync<AddTestDataCommand, bool>(new AddTestDataCommand(), currentUser).Result;
                }
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
    }
}
