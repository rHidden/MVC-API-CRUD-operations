using DavidExercise.Models;

namespace DavidExercise.Repositories.RepositoryInterfaces
{
    public interface IMemberRepository
    {
        Task<Member> CreateMember(Member member);
        Task DeleteMember(int id);
        Task UpdateMember(Member member);
        Task<Member> GetMember(int id);
    }
}
