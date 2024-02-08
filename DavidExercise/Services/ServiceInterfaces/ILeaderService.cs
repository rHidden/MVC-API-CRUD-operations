using DavidExercise.Models;

namespace DavidExercise.Services.ServiceInterfaces
{
    public interface ILeaderService
    {
        Task<Leader> GetLeader(int Id);
        Task<Leader> CreateLeader(Leader leader);
        Task UpdateLeader(Leader leader);
        Task DeleteLeader(int id);
    }
}
