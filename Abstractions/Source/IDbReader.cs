using Abstractions.Models;

namespace Abstractions.Source;

public interface IDbReader
{
    Task<IEnumerable<Database>> ScanDatabasesAsync();
    Task<IEnumerable<DatabaseTable>> ScanTablesAsync(string databaseName);
    Task<IEnumerable<dynamic>> FetchAsync(string databaseName, string tableName);
}
