global using Spectre.Console;
using LogsQuerer.Commands;
using LogsQuerer.Configuration;
using LogsQuerer.Database;
using LogsQuerer.DependencyInjection;
using LogsQuerer.Notifications;
using LogsQuerer.Query;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace LogsQuerer
{
    public class Program
    {
        static int Main(string[] args)
        {
            var dependencyInjection = CreateDependencyInjection(services =>
            {
                services.AddSingleton<IConfigFileLoader, JsonConfigFileLoader>();
                services.AddSingleton<ISqlLikeQueryParser, SqlLikeQueryParser>();
                services.AddSingleton<ISimpleQueryParser, SimpleQueryParser>();
                services.AddSingleton<IQueryParser, QueryParser>();
                services.AddSingleton<IQueryExecutor, QueryExecutor>();
                services.AddSingleton<INotificationsSender, ConsoleNotificationsSender>();
                services.AddSingleton<ISeverityLevelNotifier, SeverityLevelNotifier>();
                services.AddSingleton<IQueryLogsRepository, CsvQueryLogsRepository>();
            });

            var app = new CommandApp<QueryLogsCommand>(dependencyInjection)
                .WithDescription("A simple log querier.");

            return app.Run(args);
        }

        static ITypeRegistrar CreateDependencyInjection(Action<ServiceCollection> addDependencies)
        {
            var registrations = new ServiceCollection();

            addDependencies(registrations);

            // A type registrar is an adapter for a .NET DI framework.
            var registrar = new TypeRegistrar(registrations);

            return registrar;
        }
    }
}
