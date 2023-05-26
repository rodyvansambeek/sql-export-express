using Spectre.Console;

namespace Cli.Commands.Options;
public class DisableEscapingOption : IBoolOption
{
    public static bool TryGetPrompt(bool? value)
    {
        string questionPrompt = "[blue]?[/] Use double quotes (\") to escape string values?";
        if (value != null)
        {
            AnsiConsole.MarkupLine($"{questionPrompt} [green]{(!value.Value ? "y" : "n")}[/]");
            return value.Value;
        }

        return !AnsiConsole.Confirm(questionPrompt);
    }
}
