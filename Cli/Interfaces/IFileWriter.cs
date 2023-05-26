using Cli.Source;

namespace Cli.Interfaces;
public interface IFileWriter
{
    Task WriteFile(string filename, Table table, dynamic[] rows);
}
