namespace Abstractions.Output;
public interface IFileWriter
{
    string GetFileName(string tableName);
    Task WriteToFile(string filename, IEnumerable<dynamic> rows, OutputSettings outputSettings);
}
