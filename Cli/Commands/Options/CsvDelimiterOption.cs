using Spectre.Console;

namespace Cli.Commands.Options;
public class CsvDelimiterOption
{
    public static string TryGetPrompt(string? value)
    {
        string questionPrompt = "[blue]?[/] Which [green]CSV delimiter[/] symbol do you want to use?";
        if (value != null)
        {
            AnsiConsole.MarkupLine($"{questionPrompt} [green]{value}[/]");
            return value;
        }

        var csvDelimiterPrompt = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title(questionPrompt)
                   .AddChoices(new[] { @"Comma "",""", @"Semicolon "";""" }));
        string csvDelimiter = csvDelimiterPrompt switch
        {
            @"Comma "",""" => ",",
            @"Semicolon "";""" => ";",
            _ => throw new InvalidOperationException()
        };

        AnsiConsole.MarkupLine($"{questionPrompt} {csvDelimiter}");

        return csvDelimiter;
    }
}
