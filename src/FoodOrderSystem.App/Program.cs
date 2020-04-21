using FoodOrderSystem.Domain.Commands;
using FoodOrderSystem.Domain.Commands.EnsureAdminUser;
using FoodOrderSystem.Domain.Model.User;
using FoodOrderSystem.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

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

                var currentUser = new User(new UserId(Guid.Empty), "admin", Role.SystemAdmin, null, null);

                var commandDispatcher = services.GetService<ICommandDispatcher>();
                var result = commandDispatcher.PostAsync(new EnsureAdminUserCommand(), currentUser).Result;
            }

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var userRepository = services.GetService<IUserRepository>();
                var userFactory = services.GetService<IUserFactory>();

                for (var i = 0; i < 100; i++)
                {
                    var username = $"user{(i + 1)}";
                    var password = "Start2020!";

                    var user = userRepository.FindByNameAsync(username).Result;
                    if (user == null)
                    {
                        user = userFactory.Create(username, Role.Customer, password);
                        userRepository.StoreAsync(user).Wait();
                    }
                    else
                    {
                        user.ChangeDetails(username, Role.Customer);
                        user.ChangePassword(password);
                        userRepository.StoreAsync(user).Wait();
                    }
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
