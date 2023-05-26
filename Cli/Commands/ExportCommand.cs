using Cli.Commands.Options;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Globalization;

namespace Cli.Commands;
public class ExportCommand : AsyncCommand<ExportCommandSettings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, ExportCommandSettings settings)
    {
        string serverName = ServerNameOption.TryGetPrompt(settings.ServerName);
        string user = UserOption.TryGetPrompt(settings.User);
        string password = PasswordOption.TryGetPrompt(settings.Password);
        var tables = new[] { "users [grey](1.000 rows)[/]", "logins [grey](500 rows)[/]", "sessions [grey](123 rows)[/]" };
        string[] selectedTables = TablesOption.TryGetPrompt(tables, settings.Tables);
        string csvSeparator = CsvSeparatorOption.TryGetPrompt(settings.NonInteractive ? settings.CsvSeparator : null);
        bool disableEscaping = DisableEscapingOption.TryGetPrompt(settings.NonInteractive ? settings.DisableEscaping : null);
        CultureInfo locale = LocaleOption.TryGetPrompt(settings.NonInteractive ? settings.Locale : null);


        // Asynchronous
        await AnsiConsole.Status()
            .StartAsync("Connecting to database...", async ctx =>
            {
                // Simulate some work
                await Task.Delay(2000);
                AnsiConsole.MarkupLine("[green]Connected to database[/]");

                // Update the status and spinner
                ctx.Status("Retrieving tables...");
                ctx.Spinner(Spinner.Known.Star);
                ctx.SpinnerStyle(Style.Parse("yellow"));
                Thread.Sleep(2000);
                // Simulate some work
                AnsiConsole.MarkupLine("[green]Retrieved tables[/]");


                // Update the status and spinner
                ctx.Status("Setting up export tasks...");
                ctx.Spinner(Spinner.Known.Star);
                ctx.SpinnerStyle(Style.Parse("yellow"));
                Thread.Sleep(2000);

                // Simulate some work
                AnsiConsole.MarkupLine("[green]Initialized export tasks[/]");
            });

        // Asynchronous
        await AnsiConsole.Progress()
            .StartAsync(async ctx =>
            {
                // Define tasks
                var task1 = ctx.AddTask("[green]Exporting users[/]");
                var task2 = ctx.AddTask("[green]Exporting logins[/]");
                var task3 = ctx.AddTask("[green]Exporting sessions[/]");

                while (!ctx.IsFinished)
                {
                    // Simulate some work
                    await Task.Delay(50);

                    // Increment
                    task1.Increment(1.5);
                    task2.Increment(0.5);
                    task3.Increment(1.2);
                }
            });

        AnsiConsole.MarkupLine($"All done, export complete!");

        return 0;
    }
}
