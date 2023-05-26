using Spectre.Console;

namespace Cli.Commands.Options;
public class ServerNameOption : ITextOption
{
    public static string TryGetPrompt(string? value)
    {
        string questionPrompt = "[blue]?[/] What is the server name?";
        if (value != null)
        {
            AnsiConsole.MarkupLine($"{questionPrompt} [green]{value}[/]");
            return value;
        }

        return AnsiConsole.Ask(questionPrompt, ".");
    }
}
