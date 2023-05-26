using Spectre.Console;
using System.Text.RegularExpressions;

namespace Cli.Commands.Options;
public class TablesOption
{
    public static string[] TryGetPrompt(string[] tables, string? value)
    {
        string questionPrompt = "[blue]?[/] Which tables do you want to [green]export[/]?";
        if (value != null)
        {
            string[] values = value.Split(',');
            AnsiConsole.MarkupLine($"{questionPrompt} [green]{String.Join(", ", values)}[/]");
            return values;
        }

        var tablesPrompt =
            new MultiSelectionPrompt<string>()
                .PageSize(10)
                .Title(questionPrompt)
                .MoreChoicesText("[grey](Move up and down to reveal more tables)[/]")
                .InstructionsText("[grey](Press [blue]space[/] to toggle a table, [green]enter[/] to accept)[/]")
                .AddChoiceGroup("All Tables", tables)
                .Required();
        tablesPrompt.Select("All Tables");
        tables.ToList().ForEach(t => tablesPrompt.Select(t));

        var tablesPromptResult = AnsiConsole.Prompt(tablesPrompt);
        var selectedTables = tablesPromptResult.Select(i => Regex.Replace(i, @"\s+\[grey\]\([\d\.\,]+ rows\)\[/\]", ""));
        AnsiConsole.MarkupLine($"{questionPrompt} {string.Join(", ", selectedTables)}");

        return selectedTables.ToArray();
    }
}
