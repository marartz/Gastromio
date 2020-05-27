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
using System.IO;
using FoodOrderSystem.Domain.Commands.AddTestData;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace FoodOrderSystem.App
{
    public class Program
    {
        private static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
        
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .Enrich.FromLogContext()
                .WriteTo.Debug()
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {NewLine}{Exception}")
                .CreateLogger();
        
            try
            {
                Log.Information("Starting web host");

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
                    var result = commandDispatcher
                        .PostAsync<EnsureAdminUserCommand, bool>(new EnsureAdminUserCommand(), currentUser).Result;
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
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                .UseSerilog();
    }
}