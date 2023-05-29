using Sources.Mssql.Settings;
using System.Data.SqlClient;

namespace Sources.Mssql;
internal class DbConnection
{
    public static string CreateConnectionStringGlobal(ConnectionSettings connectionSettings)
    {
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
        {
            DataSource = connectionSettings.ServerName,
            UserID = connectionSettings.User,
            Password = connectionSettings.Password,
            IntegratedSecurity = false
        };

        return builder.ConnectionString;
    }

    public static string CreateConnectionStringDatabase(ConnectionSettings connectionSettings, string databaseName)
    {
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
        {
            DataSource = connectionSettings.ServerName,
            UserID = connectionSettings.User,
            Password = connectionSettings.Password,
            IntegratedSecurity = false,
            InitialCatalog = databaseName
        };

        return builder.ConnectionString;
    }
}
