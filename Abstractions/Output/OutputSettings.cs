using System.Globalization;

namespace Abstractions.Output;
public record OutputSettings
{
    public required string CsvDelimiter { get; set; }
    public required bool DisableEscaping { get; set; }
    public required CultureInfo Locale { get; set; }
}
