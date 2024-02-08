using DavidExercise.Models;
using DavidExercise.Repositories.RepositoryInterfaces;
using Microsoft.AspNetCore.Connections;
using Microsoft.Data.SqlClient;

namespace DavidExercise.Repositories
{
    public class MemberRepository : IMemberRepository { 
        private readonly IDatabaseConnectionFactory _connectionFactory;
        public MemberRepository(IDatabaseConnectionFactory databaseConnectionFactory)
        {
            _connectionFactory = databaseConnectionFactory;
        }
        public async Task<Member> CreateMember(Member member)
        {
            using var con = _connectionFactory.CreateConnection();
            await con.OpenAsync();
            var sql = @"INSERT INTO Member (MemberId, Name, PhoneNumber, LeaderId) VALUES
                    (@ID, @Name, @PhoneNumber, @LeaderId)";
            using (SqlCommand command = new SqlCommand(sql, con))
            {
                command.Parameters.AddWithValue("@ID", member.ID);
                command.Parameters.AddWithValue("@Name", member.Name);
                command.Parameters.AddWithValue("@PhoneNumber", member.PhoneNumber);
                command.Parameters.AddWithValue("@LeaderId", member.LeaderID);

                await command.ExecuteNonQueryAsync();
            };
            return member;
        }


        public async Task DeleteMember(int Id)
        {
            using var con = _connectionFactory.CreateConnection();
            await con.OpenAsync();

            using var transaction = con.BeginTransaction();
            var sql = @"DELETE FROM Member WHERE MemberId = @ID";

            using (SqlCommand command = new SqlCommand(sql, con, transaction))
            {
                try
                {
                    command.Parameters.AddWithValue("@ID", Id);
                    var affectedRows = await command.ExecuteNonQueryAsync();

                    if (affectedRows > 0)
                    {
                        transaction.Commit();
                        await Console.Out.WriteLineAsync($"Info: Member with ID {Id} was successfully deleted.");
                    }
                    else
                    {
                        transaction.Rollback();
                        await Console.Out.WriteLineAsync($"Warning: No Member found with ID: {Id} - no changes were made.");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    await Console.Out.WriteLineAsync($"Error: Failed to delete Member with ID: {Id}. Error: {ex.Message}");
                }
            }
        }

        public async Task<Member> GetMember(int Id)
        {
            try
            {
                using var con = _connectionFactory.CreateConnection();
                await con.OpenAsync();
                var sql = @"SELECT * FROM Member WHERE MemberId = @ID";
                using (SqlCommand command = new SqlCommand(sql, con))
                {
                    command.Parameters.AddWithValue("@ID", Id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var member = new Member
                            {
                                ID = reader.GetInt32(reader.GetOrdinal("MemberId")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                LeaderID = reader.GetInt32(reader.GetOrdinal("LeaderId"))
                            };
                            await Console.Out.WriteLineAsync($"Info: Member with ID {Id} was found.");
                            return member;
                        }
                        else
                        {
                            await Console.Out.WriteLineAsync($"Warning: Member with ID {Id} was not found.");
                            return null;
                        }
                    }
                };
            }
            catch(Exception ex) {
                throw new Exception("Error retrieving data", ex);
            }

        }

        public async Task<List<Member>> ListMembers()
        {
            using var con = _connectionFactory.CreateConnection();
            await con.OpenAsync();
            var sql = @"SELECT * FROM Member";

            using (SqlCommand command = new SqlCommand(sql, con))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    var members = new List<Member>();

                    while (await reader.ReadAsync())
                    {
                        var member = new Member
                        {
                            ID = reader.GetInt32(reader.GetOrdinal("MemberId")),
                            LeaderID = reader.GetInt32(reader.GetOrdinal("LeaderId")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber"))
                        };

                        members.Add(member);
                    }

                    if (members.Count > 0)
                    {
                        await Console.Out.WriteLineAsync($"Info: Found {members.Count} members.");
                    }
                    else
                    {
                        await Console.Out.WriteLineAsync("Warning: No members were found.");
                    }

                    return members;
                }
            }
        }

        public async Task UpdateMember(Member member)
        {
            using var con = _connectionFactory.CreateConnection();
            await con.OpenAsync();

            using var transaction = con.BeginTransaction();
            var sql = @"UPDATE Member SET Name = @Name, PhoneNumber = @PhoneNumber, LeaderId = @LeaderId WHERE MemberId = @ID";

            using (SqlCommand command = new SqlCommand(sql, con, transaction))
            {
                try
                {
                    // Prevention against SQL injection
                    command.Parameters.AddWithValue("@ID", member.ID);
                    command.Parameters.AddWithValue("@Name", member.Name ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PhoneNumber", member.PhoneNumber ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@LeaderId", member.LeaderID);

                    var affectedRows = await command.ExecuteNonQueryAsync();

                    if (affectedRows > 0)
                    {
                        transaction.Commit();
                        await Console.Out.WriteLineAsync($"Info: Member with ID: {member.ID} was successfully updated.");
                    }
                    else
                    {
                        transaction.Rollback();
                        await Console.Out.WriteLineAsync($"Warning: Member with ID: {member.ID} was not found - no changes were made.");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    await Console.Out.WriteLineAsync($"Error: Failed to update Member with ID {member.ID}. Error: {ex.Message}");
                    throw;
                }
            }
        }
    }
}
