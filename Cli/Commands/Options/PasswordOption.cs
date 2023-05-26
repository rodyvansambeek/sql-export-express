using Spectre.Console;

namespace Cli.Commands.Options;
public class PasswordOption : ITextOption
{
    public static string TryGetPrompt(string? value)
    {
        string questionPrompt = "[blue]?[/] What is the [red]password[/]?";
        if (value != null)
        {
            AnsiConsole.MarkupLine($"{questionPrompt} [green]{Enumerable.Repeat('*', value.Length)}[/]");
            return value;
        }

        return AnsiConsole.Prompt(new TextPrompt<string>(questionPrompt).PromptStyle("red").Secret());
    }
}
