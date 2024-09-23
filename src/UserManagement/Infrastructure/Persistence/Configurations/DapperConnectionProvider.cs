using Dapper;
using Npgsql;
using System.Data;

namespace UserManagement.Infrastructure.Persistence.Configurations
{
    public static class DapperConnectionProvider 
    {

        public static IDbConnection CreatePgConnection()
        {
            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.PostgreSQL);
            return new NpgsqlConnection($"Server={DatabaseSettings.Server};Port={DatabaseSettings.Port};User Id={DatabaseSettings.UserId};Password={DatabaseSettings.Password};Database={DatabaseSettings.Database};CommandTimeout={DatabaseSettings.CommandTimeout};Pooling=false");
        }
    }
}
