using DavidExercise.Models;
using DavidExercise.Repositories.RepositoryInterfaces;
using DavidExercise.Services.ServiceInterfaces;

namespace DavidExercise.Services
{
    public class LeaderService : ILeaderService
    {
        private readonly ILeaderRepository _leaderRepository;
        public LeaderService(ILeaderRepository leaderRepository)
        {
            _leaderRepository = leaderRepository;
        }
        public async Task<Leader> CreateLeader(Leader leader)
        {
            var createdLeader = await _leaderRepository.CreateLeader(leader);
            return createdLeader;
        }

        public async Task DeleteLeader(int id)
        {
            await _leaderRepository.DeleteLeader(id);
        }

        public async Task<Leader> GetLeader(int Id)
        {
            var leader = await _leaderRepository.GetLeader(Id);
            return leader;
        }

        public async Task UpdateLeader(Leader leader)
        {
            await _leaderRepository.UpdateLeader(leader);
        }
    }
}
