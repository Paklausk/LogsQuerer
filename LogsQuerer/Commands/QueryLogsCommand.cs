using LogsQuerer.Configuration;
using LogsQuerer.Database;
using LogsQuerer.Logs;
using LogsQuerer.Notifications;
using LogsQuerer.Query;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace LogsQuerer.Commands
{
    [Description("Query logs.")]
    public sealed class QueryLogsCommand(
        IConfigFileLoader configFileLoader,
        IQueryExecutor queryExecutor,
        ISeverityLevelNotifier severityLevelNotifier,
        IQueryLogsRepository queryLogsRepository
    ) : Command<QueryLogsCommand.Settings>
    {
        public enum RunMode
        {
            Query,
            Loop
        }

        public sealed class Settings : CommandSettings
        {
            [Description("Query text to output specific filtered logs")]
            [CommandOption("-q|--query <QUERY>")]
            public string? Query { get; init; }

            [Description("Path to config file. Defaults to current directory 'config.json'.")]
            [CommandOption("-c|--config <CONFIG_PATH>")]
            [DefaultValue("config.json")]
            public string? ConfigPath { get; init; }

            public RunMode Mode => Query is not null ? RunMode.Query : RunMode.Loop;
        }

        public override ValidationResult Validate(CommandContext context, Settings settings)
        {
            if (string.IsNullOrWhiteSpace(settings.ConfigPath))
            {
                return ValidationResult.Error("Config path is required.");
            }

            return base.Validate(context, settings);
        }

        public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
        {
            var config = configFileLoader.Load(settings.ConfigPath!);

            if (config.LogFiles is null || config.LogFiles.Count() == 0)
            {
                AnsiConsole.MarkupLine("[bold red]No log files found in the configuration[/]");
                return 1;
            }

            var logFilesProvider = new CsvLogFilesProvider(config.LogFiles);

            switch (settings.Mode)
            {
                case RunMode.Query:
                    ExecuteQuery(config, logFilesProvider, settings.Query!);
                    break;
                case RunMode.Loop:
                    StartLoopMode(config, logFilesProvider);
                    break;
            }

            return 0;
        }

        void StartLoopMode(Config config, ILogFilesProvider logFilesProvider)
        {
            while (true)
            {
                AnsiConsole.MarkupLine("Enter your query or press [bold yellow]Ctrl + C[/] to exit");

                var query = AnsiConsole.Prompt(new TextPrompt<string?>("Enter your query:"));

                if (string.IsNullOrWhiteSpace(query))
                {
                    break;
                }

                ExecuteQuery(config, logFilesProvider, query);

                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[bold]Press [yellow]Enter[/] to continue[/]");
                Console.ReadLine();

                AnsiConsole.Clear();
            }
        }

        void ExecuteQuery(Config config, ILogFilesProvider logFilesProvider, string query)
        {
            var result = queryExecutor.ExecuteQuery(query, logFilesProvider);

            queryLogsRepository.AuditQuery(query, result.Status, result.Logs?.Length ?? 0);

            switch (result.Status)
            {
                case QueryResultStatus.ColumnNotFound:
                    AnsiConsole.MarkupLine($"[bold red]Column not found in the log files[/]");
                    break;

                case QueryResultStatus.InvalidQuery:
                    AnsiConsole.MarkupLine($"[bold red]Invalid query '{EscapeMarkup(query)}'[/]");
                    break;

                case QueryResultStatus.Success:

                    if (result.Logs == null || result.Logs.Length == 0)
                    {
                        AnsiConsole.MarkupLine("[bold yellow]No logs found[/]");
                        break;
                    }

                    OutputLogs(result.Logs);

                    if (config.AlertSeverityLevel.HasValue)
                    {
                        severityLevelNotifier.CheckSeverityLevelAndNotify(result.Logs, config.AlertSeverityLevel.Value);
                    }

                    AnsiConsole.MarkupLine($"[bold]Found [yellow]{result.Logs.Length}[/] logs[/]");
                    break;
            }
        }

        void OutputLogs(Log[] logs)
        {
            foreach (var log in logs)
            {
                AnsiConsole.WriteLine();

                var table = new Table();

                table.AddColumn("Column");
                table.AddColumn("Log");

                foreach (var field in log.Fields)
                {
                    table.AddRow(EscapeMarkup(field.Key), EscapeMarkup(field.Value));
                }

                AnsiConsole.Write(table);
            }
        }

        string EscapeMarkup(string text)
        {
            return text.Replace("[", "[[").Replace("]", "]]");
        }
    }
}
