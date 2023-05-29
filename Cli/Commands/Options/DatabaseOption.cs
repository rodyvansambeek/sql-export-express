using Spectre.Console;

namespace Cli.Commands.Options;
public class DatabaseOption
{
    public static string TryGetPrompt(string[] databases, string? value)
    {
        string questionPrompt = "[blue]?[/] Which database do you want to [green]export[/]?";
        if (value != null)
        {
            string[] values = value.Split(',');
            AnsiConsole.MarkupLine($"{questionPrompt} [green]{String.Join(", ", value)}[/]");
            return value;
        }

        var databasesPrompt =
            new SelectionPrompt<string>()
                .AddChoices(databases)
                .PageSize(10)
                .Title(questionPrompt)
                .MoreChoicesText("[grey](Move up and down to reveal more databases)[/]");

        var databasesPromptResult = AnsiConsole.Prompt(databasesPrompt);
        AnsiConsole.MarkupLine($"{questionPrompt} {databasesPromptResult}");

        return databasesPromptResult;
    }
}
