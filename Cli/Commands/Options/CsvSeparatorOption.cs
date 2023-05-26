using Spectre.Console;

namespace Cli.Commands.Options;
public class CsvSeparatorOption
{
    public static string TryGetPrompt(string? value)
    {
        string questionPrompt = "[blue]?[/] Which [green]CSV separator[/] symbol do you want to use?";
        if (value != null)
        {
            AnsiConsole.MarkupLine($"{questionPrompt} [green]{value}[/]");
            return value;
        }

        var csvSeparatorPrompt = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title(questionPrompt)
                   .AddChoices(new[] { @"Comma "",""", @"Semicolon "";""" }));
        string csvSeparator = csvSeparatorPrompt switch
        {
            @"Comma "",""" => ",",
            @"Semicolon "";""" => ";",
            _ => throw new InvalidOperationException()
        };

        AnsiConsole.MarkupLine($"{questionPrompt} {csvSeparator}");

        return csvSeparator;
    }
}
