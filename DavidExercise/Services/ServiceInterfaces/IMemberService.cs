using DavidExercise.Models;

namespace DavidExercise.Services.ServiceInterfaces
{
    public interface IMemberService
    {
        Task<Member> CreateMember(Member member);
        Task DeleteMember(int id);
        Task UpdateMember(Member member);
        Task<Member> GetMember(int id);
        Task<Leader> PromoteMember(int id);
        Task<List<Member>> ListMembers();

    }
}
