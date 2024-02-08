using DavidExercise.Models;
using DavidExercise.Repositories;
using DavidExercise.Repositories.RepositoryInterfaces;
using DavidExercise.Services.ServiceInterfaces;

namespace DavidExercise.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly ILeaderRepository _leaderRepository;

        public MemberService(IMemberRepository memberRepository, ILeaderRepository leaderRepository)
        {
            _memberRepository = memberRepository;
            _leaderRepository = leaderRepository;
        }
        public async Task<Member> CreateMember(Member member)
        {
            return await _memberRepository.CreateMember(member);
        }

        public async Task DeleteMember(int Id)
        {
            await _memberRepository.DeleteMember(Id);

        }

        public async Task<Member> GetMember(int Id)
        {
            return await _memberRepository.GetMember(Id);
        }

        public async Task UpdateMember(Member member)
        {
            await _memberRepository.UpdateMember(member);
        }

        public async Task<Leader> PromoteMember(int Id)
        {
            try
            {
                var member = await _memberRepository.GetMember(Id);
                var leader = new Leader
                {
                    ID = member.ID,
                    PhoneNumber = member.PhoneNumber,
                    Name = member.Name
                };
                var createdLeader = await _leaderRepository.CreateLeader(leader);
                await _memberRepository.DeleteMember(Id);
                return createdLeader;
            }
            catch (Exception ex)
            {
                throw new Exception("Error promoting member to leader", ex);
            }
        }
    }
}
