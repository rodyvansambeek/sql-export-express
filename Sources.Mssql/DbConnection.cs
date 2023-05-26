using Sources.Mssql.Settings;
using System.Data.SqlClient;

namespace Sources.Mssql;
internal class DbConnection
{
    public static string CreateConnectionString(ConnectionSettings connectionSettings)
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
}
