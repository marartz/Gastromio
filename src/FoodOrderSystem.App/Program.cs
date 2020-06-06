using FoodOrderSystem.Domain.Commands;
using FoodOrderSystem.Domain.Commands.EnsureAdminUser;
using FoodOrderSystem.Domain.Model.User;
using FoodOrderSystem.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using FoodOrderSystem.Domain.Commands.AddTestData;
using FoodOrderSystem.Domain.Model;
using FoodOrderSystem.Domain.Services;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace FoodOrderSystem.App
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                Log.Information("Starting web host");

                var host = CreateHostBuilder(args).Build();

                // using (var scope = host.Services.CreateScope())
                // {
                //     var services = scope.ServiceProvider;
                //     var dbContext = services.GetService<SystemDbContext>();
                //     dbContext.Database.Migrate();
                // }

                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;

                    var configuration = services.GetService<IConfiguration>();

                    var currentUser = new User(new UserId(Guid.Empty), Role.SystemAdmin, "admin@gastromio.de", null, null);
                    var commandDispatcher = services.GetService<ICommandDispatcher>();

                    var failureMessageService = services.GetService<IFailureMessageService>();
                    
                    var seed =
                        string.Equals(configuration["Seed"], "true", StringComparison.InvariantCultureIgnoreCase) ||
                        configuration.GetSection("Seed").GetChildren().Any();
                    if (seed)
                    {
                        Persistence.MongoDB.Initializer.PurgeDatabase(services);
                        Persistence.MongoDB.Initializer.PrepareDatabase(services);

                        var result = commandDispatcher
                            .PostAsync<EnsureAdminUserCommand, bool>(new EnsureAdminUserCommand(), currentUser).Result;
                        if (result.IsFailure)
                        {
                            var failureResult = (FailureResult<bool>) result;
                            Log.Logger.Error(string.Join("; ", failureResult.Errors));
                            throw new InvalidOperationException("Error during command EnsureAdminUserCommand");
                        }
                        
                        if (!Int32.TryParse(configuration["Seed:Params:UserCount"], out var userCount))
                            userCount = 20;

                        if (!Int32.TryParse(configuration["Seed:Params:RestCount"], out var restCount))
                            restCount = 15;

                        if (!Int32.TryParse(configuration["Seed:Params:DishCatCount"], out var dishCatCount))
                            dishCatCount = 8;

                        if (!Int32.TryParse(configuration["Seed:Params:DishCount"], out var dishCount))
                            dishCount = 8;

                        result = commandDispatcher
                            .PostAsync<AddTestDataCommand, bool>(
                                new AddTestDataCommand(userCount, restCount, dishCatCount, dishCount), currentUser)
                            .Result;
                        if (result.IsFailure)
                        {
                            var failureResult = (FailureResult<bool>) result;
                            Log.Logger.Error(string.Join("; ", failureResult.Errors.Values.SelectMany(en => en)));
                            throw new InvalidOperationException("Error during command AddTestDataCommand");
                        }
                    }
                    else
                    {
                        Persistence.MongoDB.Initializer.PrepareDatabase(services);

                        var result = commandDispatcher
                            .PostAsync<EnsureAdminUserCommand, bool>(new EnsureAdminUserCommand(), currentUser).Result;
                        if (result.IsFailure)
                        {
                            var failureResult = (FailureResult<bool>) result;
                            Log.Logger.Error(string.Join("; ", failureResult.Errors));
                            throw new InvalidOperationException("Error during command EnsureAdminUserCommand");
                        }
                    }
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