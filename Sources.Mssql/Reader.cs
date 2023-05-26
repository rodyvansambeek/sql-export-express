using Cli.Interfaces;
using Cli.Source;
using Dapper;
using Sources.Mssql.Settings;
using System.Data.SqlClient;

namespace Sources.Mssql;
public class Reader : IDbReader
{
    private readonly ConnectionSettings _connectionSettings;
    private readonly string _connectionString;

    public Reader(ConnectionSettings connectionSettings)
    {
        _connectionSettings = connectionSettings;
        _connectionString = DbConnection.CreateConnectionString(_connectionSettings);
    }

    public async Task<IEnumerable<Table>> ScanTablesAsync()
    {
        using var connection = new SqlConnection(_connectionString);
        var sql = @"""
            SELECT
                  sOBJ.name AS [Name], SUM(sdmvPTNS.row_count) AS [Rows]
            FROM
                  sys.objects AS sOBJ
                  INNER JOIN sys.dm_db_partition_stats AS sdmvPTNS
                        ON sOBJ.object_id = sdmvPTNS.object_id
            WHERE 
                  sOBJ.type = 'U'
                  AND sOBJ.is_ms_shipped = 0x0
                  AND sdmvPTNS.index_id < 2
            GROUP BY
                  sOBJ.schema_id, 
	              sOBJ.name
            ORDER BY [TableName]
            GO""";

        var tables = await connection.QueryAsync<Table>(sql);
        return tables;
    }

    public Task<dynamic[]> FetchAsync(Table table, int skip, int take)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ConnectToDatabase()
    {
        using var connection = new SqlConnection(_connectionString);
        return Task.FromResult(true);
    }
}
