using System.Data.Common;
using System.IO;

namespace Lyra.Models.Database
{
    public static class DatabaseConnectionProvider
    {
        public static DbConnection GetConnection()
        {
            var connection = DbProviderFactories.GetFactory(LyraApp.DatabaseProvider).CreateConnection();
            if (connection == null)
                throw new IOException();

            connection.ConnectionString = LyraApp.DatabaseConnectionString;
            return connection;
        }
    }
}