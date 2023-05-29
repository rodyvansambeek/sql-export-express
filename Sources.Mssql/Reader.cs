using Abstractions.Models;
using Abstractions.Source;
using Dapper;
using Sources.Mssql.Settings;
using System.Data.SqlClient;

namespace Sources.Mssql;
public class Reader : IDbReader
{
    private readonly ConnectionSettings _connectionSettings;

    public Reader(ConnectionSettings connectionSettings)
    {
        _connectionSettings = connectionSettings;
    }

    public async Task<IEnumerable<Database>> ScanDatabasesAsync()
    {
        string connectionString = DbConnection.CreateConnectionStringGlobal(_connectionSettings);
        using var connection = new SqlConnection(connectionString);
        var sql = """
            SELECT [name]
            FROM master.sys.databases
            WHERE Cast(CASE WHEN name IN('master', 'model', 'msdb', 'tempdb') THEN 1 ELSE is_distributor END As bit) = 0
            """;

        var databases = await connection.QueryAsync<Database>(sql);
        return databases;
    }

    public async Task<IEnumerable<DatabaseTable>> ScanTablesAsync(string databaseName)
    {
        string connectionString = DbConnection.CreateConnectionStringDatabase(_connectionSettings, databaseName);
        using var connection = new SqlConnection(connectionString);
        var sql = """
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
            ORDER BY [Name]
            """;

        var tables = await connection.QueryAsync<DatabaseTable>(sql);
        return tables;
    }

    public async Task<IEnumerable<dynamic>> FetchAsync(string databaseName, string tableName)
    {
        string connectionString = DbConnection.CreateConnectionStringDatabase(_connectionSettings, databaseName);
        using var connection = new SqlConnection(connectionString);

        string tableNameExistsCheck = "SELECT count(1) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @tableName";
        int tableCount = await connection.ExecuteScalarAsync<int>(tableNameExistsCheck, new { tableName });
        if (tableCount != 1)
        {
            throw new ArgumentException($"Table '{tableName}' does not exist");
        }

        string sql = string.Format("Select * From [{0}]", tableName);
        return await connection.QueryAsync<dynamic>(sql);
    }
}
