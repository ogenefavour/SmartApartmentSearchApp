using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nest;
using Serilog;
using Smart.Apartment.Application;
using Smart.Apartment.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart.Apartment.UploadService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IElasticClient>(sp =>
                    {
                        var config = sp.GetRequiredService<IConfiguration>();
                        var settings = new ConnectionSettings(new Uri(config["EndPoints:ElasticSearchEndpoint"])).DefaultIndex("apartments");

                        return new ElasticClient(settings);
                    });
                    services.AddApplicationServices()
                    .AddInfrastructureServices();
                    services.AddHostedService<Worker>();
                });
    }
}
