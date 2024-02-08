using Microsoft.Data.SqlClient;
using System.Data;

namespace DavidExercise.Repositories.RepositoryInterfaces
{
    public interface IDatabaseConnectionFactory
    {
        public SqlConnection CreateConnection();
    }
}
