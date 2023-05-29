using Abstractions.Output;
using CsvHelper;
using CsvHelper.Configuration;

namespace Outputs.Csv;

public class Writer : IFileWriter
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

    public async Task WriteToFile(string filename, IEnumerable<dynamic> rows, OutputSettings outputSettings)
    {
        using var writer = new StreamWriter(filename);

        new CsvConfiguration(outputSettings.Locale)
        {
            Delimiter = outputSettings.CsvDelimiter,
            Escape = outputSettings.DisableEscaping ? '\0' : '"',
            HasHeaderRecord = true,
        };
        using var csv = new CsvWriter(writer, outputSettings.Locale);
        await csv.WriteRecordsAsync(rows);
    }
}
