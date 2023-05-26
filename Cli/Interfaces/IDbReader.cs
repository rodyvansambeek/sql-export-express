using Cli.Source;

namespace Cli.Interfaces;
public interface IDbReader
{
    Task<bool> ConnectToDatabase();
    Task<IEnumerable<Table>> ScanTablesAsync();
    Task<dynamic[]> FetchAsync(Table table, int skip, int take);
}
