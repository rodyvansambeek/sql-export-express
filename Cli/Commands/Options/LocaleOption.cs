using Spectre.Console;
using System.Globalization;

namespace Cli.Commands.Options;
public class LocaleOption
{
    public static CultureInfo TryGetPrompt(string? value)
    {
        string questionPrompt = "[blue]?[/] Which [green]locale[/] do you want your export to use?";
        if (value != null)
        {
            AnsiConsole.MarkupLine($"{questionPrompt} [green]{value}[/]");
            return new CultureInfo(value);
        }

        CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
        var localePrompt = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title(questionPrompt)
                   .AddChoices(new[] { $"Invariant (default)" })
                   .AddChoices(new[] { $"Current system locale ({CultureInfo.CurrentCulture.Name})" })
                   .AddChoices(cultures.Where(i => !string.IsNullOrWhiteSpace(i.Name)).Select(i => i.Name)));
        CultureInfo locale = localePrompt.StartsWith("Current system locale") ? CultureInfo.CurrentCulture : (localePrompt.StartsWith("Invariant") ? CultureInfo.InvariantCulture : CultureInfo.GetCultureInfo(localePrompt));
        AnsiConsole.MarkupLine($"{questionPrompt} {(locale == CultureInfo.InvariantCulture ? "Invariant" : locale.Name)}");

        return locale;
    }
}
