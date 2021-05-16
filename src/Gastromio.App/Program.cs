using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using Gastromio.Core.Application.Commands.AddTestData;
using Gastromio.Core.Application.Commands.EnsureAdminUser;
using Gastromio.Core.Domain.Model.Users;
using Gastromio.Persistence.MongoDB;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace Gastromio.App
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("./logs/log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            try
            {
                Log.Information("Starting web host");

                var host = CreateHostBuilder(args).Build();

                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;

                    var configuration = services.GetService<IConfiguration>();

                    var dbAdminService = services.GetService<IDbAdminService>();

                    var currentUserId = new UserId(Guid.NewGuid());

                    var currentUser = new User(
                        currentUserId,
                        Role.SystemAdmin,
                        "admin@gastromio.de",
                        null,
                        null,
                        null,
                        null,
                        DateTimeOffset.UtcNow,
                        currentUserId,
                        DateTimeOffset.UtcNow,
                        currentUserId
                    );

                    var ensureAdminUserCommandHandler = services.GetService<EnsureAdminUserCommandHandler>();

                    var seed =
                        string.Equals(configuration["Seed"], "true", StringComparison.InvariantCultureIgnoreCase) ||
                        configuration.GetSection("Seed").GetChildren().Any();
                    if (seed)
                    {
                        dbAdminService.PurgeDatabase();
                        dbAdminService.PrepareDatabase();

                        ensureAdminUserCommandHandler.HandleAsync(new EnsureAdminUserCommand(), currentUser).GetAwaiter().GetResult();

                        if (!Int32.TryParse(configuration["Seed:Params:UserCount"], out var userCount))
                            userCount = 20;

                        if (!Int32.TryParse(configuration["Seed:Params:RestCount"], out var restCount))
                            restCount = 15;

                        if (!Int32.TryParse(configuration["Seed:Params:DishCatCount"], out var dishCatCount))
                            dishCatCount = 8;

                        if (!Int32.TryParse(configuration["Seed:Params:DishCount"], out var dishCount))
                            dishCount = 8;

                        var addTestDataCommandHandler = services.GetService<AddTestDataCommandHandler>();

                        addTestDataCommandHandler.HandleAsync(
                            new AddTestDataCommand(userCount, restCount, dishCatCount, dishCount), currentUser).GetAwaiter().GetResult();;
                    }
                    else
                    {
                        dbAdminService.PrepareDatabase();

                        ensureAdminUserCommandHandler.HandleAsync(new EnsureAdminUserCommand(), currentUser).GetAwaiter().GetResult();;
                    }

                    dbAdminService.CorrectRestaurantAliases();
                }

                host.Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) => { config.AddCommandLine(args); })
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}
