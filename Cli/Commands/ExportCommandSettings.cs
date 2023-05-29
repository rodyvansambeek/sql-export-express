using Spectre.Console.Cli;
using System.ComponentModel;

namespace Cli.Commands;
public class ExportCommandSettings : CommandSettings
{
    [CommandOption("-n|--non-interactive")]
    [Description("Run the application without requiring any user input or interaction.")]
    [DefaultValue(false)]
    public bool NonInteractive { get; set; }

    [CommandOption("-s|--server-name <SERVER_NAME>")]
    public string? ServerName { get; set; }

    [CommandOption("-u|--user <USER>")]
    public string? User { get; set; }

    [CommandOption("-p|--password <PASSWORD>")]
    public string? Password { get; set; }

    [CommandOption("-t|--tables <TABLE1>")]
    public string? Tables { get; set; }

    [CommandOption("-a|--all-tables")]
    public bool? AllTables { get; set; }

    [CommandOption("-c|--csv-delimiter <SYMBOL>")]
    [Description("The CSV delimiter symbol to use")]
    [DefaultValue(",")]
    public string? CsvDelimiter { get; set; }

    [CommandOption("-d|--disable-escaping")]
    [Description("Disable the use of double quotes (\"...\") to escape string values")]
    [DefaultValue(false)]
    public bool? DisableEscaping { get; set; }

    [CommandOption("-l|--locale <LOCALE>")]
    [Description("The locale used to export the values")]
    [DefaultValue("Invariant locale")]
    public string? Locale { get; set; }
}

