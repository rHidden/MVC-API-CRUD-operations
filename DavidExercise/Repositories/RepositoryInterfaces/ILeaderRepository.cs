using DavidExercise.Models;

namespace DavidExercise.Repositories.RepositoryInterfaces
{
    public interface ILeaderRepository
    {
        Task<Leader> GetLeader(int Id);
        Task<Leader> CreateLeader(Leader leader);
        Task UpdateLeader(Leader leader);
        Task DeleteLeader(int id);
    }
}
