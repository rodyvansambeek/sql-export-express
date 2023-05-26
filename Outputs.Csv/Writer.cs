using Cli.Interfaces;
using Cli.Source;
using CsvHelper;
using System.Globalization;

namespace Outputs.Csv;

public class Writer : IFileWriter
{
    public async Task WriteFile(string filename, Table table, dynamic[] rows)
    {
        using var writer = new StreamWriter(filename);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        await csv.WriteRecordsAsync(rows);
    }
}
