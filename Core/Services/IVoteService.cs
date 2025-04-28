using Core.Abestraction;
using Core.Contracts.Vote;

namespace Core.Services
{
    public interface IVoteService
    {
        Task<Result> AddAsync(int pollId, string userId, VoteRequest request, CancellationToken cancellationToken);
    }
}
