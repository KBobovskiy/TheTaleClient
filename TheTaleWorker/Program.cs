using DataBaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using System;

namespace TheTaleWorker
{
    public class Program
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args);

            var builder = host.Build();

            _logger.Info($"Run worker");
            builder.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddEnvironmentVariables(prefix: "TheTale_");
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();

                    IConfiguration configuration = hostContext.Configuration;

                    WorkerConfiguration workerConfiguration = configuration.GetSection(nameof(WorkerConfiguration)).Get<WorkerConfiguration>();
                    if (workerConfiguration == null)
                    {
                        _logger.Error($"{nameof(WorkerConfiguration)} is NULL! Set up secrets, for more info see AddSecrets.txt");
                        throw new NullReferenceException($"{nameof(WorkerConfiguration)} is NULL!");
                    }

                    services.AddSingleton(workerConfiguration);
                    services.AddDbContext<SqliteContext>(options => options.UseSqlite(workerConfiguration.ConnectionString), ServiceLifetime.Transient, ServiceLifetime.Transient);

                    _logger.Info($"ConfigureServices complete");
                });
    }
}