using Abstractions.Models;
using Abstractions.Output;
using Abstractions.Source;
using Cli.Commands.Options;
using Outputs.Csv;
using Sources.Mssql;
using Sources.Mssql.Settings;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Globalization;

namespace Cli.Commands;
public class ExportCommand : AsyncCommand<ExportCommandSettings>
{
    private ConnectionSettings? _connectionSettings;
    private IDbReader? _dbReader;
    private readonly IFileWriter writer = new Writer();

    public override async Task<int> ExecuteAsync(CommandContext context, ExportCommandSettings settings)
    {
        _connectionSettings = PromptConnectionSettings(settings);
        _dbReader = new Reader(_connectionSettings);

        string selectedDatabase = await PromptDatabase(settings);
        DatabaseTable[] selectedTableObjects = await PromptTables(settings, selectedDatabase);
        OutputSettings outputSettings = PromptOutputSettings(settings);

        await Export(selectedDatabase, selectedTableObjects, outputSettings);

        return 0;
    }

    private async Task Export(string selectedDatabase, DatabaseTable[] selectedTableObjects, OutputSettings outputSettings)
    {
        await AnsiConsole.Progress()
            .AutoClear(true)
            .StartAsync(async ctx =>
            {
                // Define tasks
                var exportTasks = new List<Task>();
                foreach (var table in selectedTableObjects)
                {
                    var progressTask = ctx.AddTask($"[green]Exporting {table.Name}[/]");
                    exportTasks.Add(CreateExportTask(selectedDatabase, table, progressTask, outputSettings));
                }

                await Task.WhenAll(exportTasks.ToArray());
            });

        AnsiConsole.WriteLine();

        foreach (var table in selectedTableObjects)
        {
            AnsiConsole.MarkupLine($"Exported table [green]{table.Name}[/] to file [green]{writer.GetFileName(table.Name)}[/]");
        }

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine($"All done, export complete!");
        AnsiConsole.MarkupLine($"Like this utility? Please [link=https://github.com/rodyvansambeek/sql-export-express]give us a star on GitHub[/]!");
    }

    private OutputSettings PromptOutputSettings(ExportCommandSettings settings)
    {
        string csvDelimiter = CsvDelimiterOption.TryGetPrompt(settings.NonInteractive ? settings.CsvDelimiter : null);
        bool disableEscaping = DisableEscapingOption.TryGetPrompt(settings.NonInteractive ? settings.DisableEscaping : null);
        CultureInfo locale = LocaleOption.TryGetPrompt(settings.NonInteractive ? settings.Locale : null);

        return new OutputSettings
        {
            CsvDelimiter = csvDelimiter,
            DisableEscaping = disableEscaping,
            Locale = locale
        };
    }

    private async Task<DatabaseTable[]> PromptTables(ExportCommandSettings settings, string selectedDatabase)
    {
        ArgumentNullException.ThrowIfNull(_dbReader);

        var dbTables = await _dbReader.ScanTablesAsync(selectedDatabase);
        var tableSelections = dbTables.Select(i => $"{i.Name} [grey]({i.Rows.ToString(CultureInfo.InvariantCulture)} rows)[/]").ToArray();
        var selectedTables = TablesOption.TryGetPrompt(tableSelections, settings.Tables);
        DatabaseTable[] selectedTableObjects = dbTables.Where(i => selectedTables.Contains(i.Name)).ToArray();

        return selectedTableObjects;
    }

    private async Task<string> PromptDatabase(ExportCommandSettings settings)
    {
        ArgumentNullException.ThrowIfNull(_dbReader);

        var dbDatabases = await _dbReader.ScanDatabasesAsync();
        var databaseSelections = dbDatabases.Select(i => i.Name).ToArray();
        string selectedDatabase = DatabaseOption.TryGetPrompt(databaseSelections, settings.Tables);

        return selectedDatabase;
    }

    private ConnectionSettings PromptConnectionSettings(ExportCommandSettings settings)
    {
        string serverName = ServerNameOption.TryGetPrompt(settings.ServerName);
        string user = UserOption.TryGetPrompt(settings.User);
        string password = PasswordOption.TryGetPrompt(settings.Password);
        return new ConnectionSettings
        {
            ServerName = serverName,
            User = user,
            Password = password
        };
    }

    private async Task CreateExportTask(string databaseName, DatabaseTable table, ProgressTask progressTask, OutputSettings outputSettings)
    {
        ArgumentNullException.ThrowIfNull(_dbReader);
        string filename = writer.GetFileName(table.Name);

        var rows = await _dbReader.FetchAsync(databaseName, table.Name);
        int chunkSize = 100;
        double chunkPercentage = Math.Max(100f / (table.Rows / (double)chunkSize), 100f);
        foreach (var chunk in rows.Chunk(chunkSize))
        {
            await writer.WriteToFile(filename, chunk, outputSettings);
            progressTask.Increment(chunkPercentage);
        }
    }
}
