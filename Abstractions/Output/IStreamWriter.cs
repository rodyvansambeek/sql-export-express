namespace Abstractions.Output;
public interface IStreamWriter
{
    string GetFileName(string tableName);
    Task Write(StreamWriter writer, IEnumerable<dynamic> rows, OutputSettings outputSettings);
}
