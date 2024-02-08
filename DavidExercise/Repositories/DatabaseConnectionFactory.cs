using DavidExercise.Repositories.RepositoryInterfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DavidExercise.Repositories
{
    public class DatabaseConnectionFactory : IDatabaseConnectionFactory
    {
        private readonly string _connectionString;
        public DatabaseConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }
        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
