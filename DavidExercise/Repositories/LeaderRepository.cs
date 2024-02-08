using DavidExercise.Models;
using DavidExercise.Repositories.RepositoryInterfaces;
using Microsoft.Data.SqlClient;

namespace DavidExercise.Repositories
{
    public class LeaderRepository : ILeaderRepository
    {
        private readonly IDatabaseConnectionFactory _connectionFactory;
        public LeaderRepository(IDatabaseConnectionFactory databaseConnectionFactory)
        {
            _connectionFactory = databaseConnectionFactory;
        }
        public async Task<Leader> CreateLeader(Leader leader)
        {
            using var con = _connectionFactory.CreateConnection();
            await con.OpenAsync();
            var sql = @"INSERT INTO Leader (LeaderId, Name, PhoneNumber) VALUES
                (@ID, @Name, @PhoneNumber)";
            using (SqlCommand command = new SqlCommand(sql, con))
            {
                command.Parameters.AddWithValue("@ID", leader.ID);
                command.Parameters.AddWithValue("@Name", leader.Name);
                command.Parameters.AddWithValue("@PhoneNumber", leader.PhoneNumber);

                await command.ExecuteNonQueryAsync();
            };
            return leader;
        }


        public async Task DeleteLeader(int Id)
        {
            using var con = _connectionFactory.CreateConnection();
            await con.OpenAsync();

            using var transaction = con.BeginTransaction();
            var sql = @"DELETE FROM Leader WHERE LeaderId = @ID";

            using (SqlCommand command = new SqlCommand(sql, con, transaction))
            {
                try
                {
                    command.Parameters.AddWithValue("@ID", Id);
                    var affectedRows = await command.ExecuteNonQueryAsync();

                    if (affectedRows > 0)
                    {
                        transaction.Commit();
                        await Console.Out.WriteLineAsync($"Info: Leader with ID {Id} was successfully deleted.");
                    }
                    else
                    {
                        transaction.Rollback();
                        await Console.Out.WriteLineAsync($"Warning: No leader found with ID: {Id} - no changes were made.");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    await Console.Out.WriteLineAsync($"Error: Failed to delete leader with ID: {Id}. Error: {ex.Message}");
                }
            }
        }

        public async Task<Leader> GetLeader(int Id)
        {
            using var con = _connectionFactory.CreateConnection();
            await con.OpenAsync();
            var sql = @"SELECT * FROM Leader WHERE LeaderId = @ID";
            using (SqlCommand command = new SqlCommand(sql, con))
            {
                command.Parameters.AddWithValue("@ID", Id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        var leader = new Leader
                        {
                            ID = reader.GetInt32(reader.GetOrdinal("LeaderId")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber"))
                        };
                        await Console.Out.WriteLineAsync($"Info: Leader with ID {Id} was found.");
                        return leader;
                    }
                    else
                    {
                        await Console.Out.WriteLineAsync($"Warning: Leader with ID {Id} was not found.");
                        return null;
                    }
                }
            };
        }

        public async Task<List<Leader>> ListLeaders()
        {
            using var con = _connectionFactory.CreateConnection();
            await con.OpenAsync();
            var sql = @"SELECT * FROM Leader";

            using (SqlCommand command = new SqlCommand(sql, con))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    var leaders = new List<Leader>();

                    while (await reader.ReadAsync())
                    {
                        var leader = new Leader
                        {
                            ID = reader.GetInt32(reader.GetOrdinal("LeaderId")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber"))
                        };

                        leaders.Add(leader);
                    }

                    if (leaders.Count > 0)
                    {
                        await Console.Out.WriteLineAsync($"Info: Found {leaders.Count} leaders.");
                    }
                    else
                    {
                        await Console.Out.WriteLineAsync("Warning: No leaders were found.");
                    }

                    return leaders;
                }
            }
        }


        public async Task UpdateLeader(Leader leader)
        {
            using var con = _connectionFactory.CreateConnection();
            await con.OpenAsync();

            using var transaction = con.BeginTransaction();
            var sql = @"UPDATE Leader SET Name = @Name, PhoneNumber = @PhoneNumber WHERE LeaderId = @ID";

            using (SqlCommand command = new SqlCommand(sql, con, transaction))
            {
                try
                {
                    // Prevention against SQL injection
                    command.Parameters.AddWithValue("@ID", leader.ID);
                    command.Parameters.AddWithValue("@Name", leader.Name ?? (object)DBNull.Value); 
                    command.Parameters.AddWithValue("@PhoneNumber", leader.PhoneNumber ?? (object)DBNull.Value);

                    var affectedRows = await command.ExecuteNonQueryAsync();

                    if (affectedRows > 0)
                    {
                        transaction.Commit();
                        await Console.Out.WriteLineAsync($"Info: Leader with ID: {leader.ID} was successfully updated.");
                    }
                    else
                    {
                        transaction.Rollback();
                        await Console.Out.WriteLineAsync($"Warning: Leader with ID: {leader.ID} was not found - no changes were made.");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    await Console.Out.WriteLineAsync($"Error: Failed to update leader with ID {leader.ID}. Error: {ex.Message}");
                    throw;
                }
            }
        }
    }
}
