using Abstractions.Output;
using CsvHelper;
using CsvHelper.Configuration;

namespace Outputs.Csv;

public class Writer : IStreamWriter
{
    public string GetFileName(string tableName)
    {
        string invalidChars = new string(Path.GetInvalidFileNameChars());
        string safeFilename = tableName;

        foreach (char invalidChar in invalidChars)
        {
            safeFilename = safeFilename.Replace(invalidChar.ToString(), "");
        }

        return $"{safeFilename}.csv";
    }

    public async Task Write(StreamWriter writer, IEnumerable<dynamic> rows, OutputSettings outputSettings)
    {
        var configuration = new CsvConfiguration(outputSettings.Locale)
        {
            Delimiter = outputSettings.CsvDelimiter,
            Escape = outputSettings.DisableEscaping ? '\0' : '"',
            HasHeaderRecord = writer.BaseStream.Position == 0,
        };
        using var csv = new CsvWriter(writer, configuration, true);
        await csv.WriteRecordsAsync(rows);
    }
}
