using Busniss.Persistence;
using Core.Abestraction;
using Core.Abestraction.Errors;
using Core.Contracts.Vote;
using Core.Services;
using Mapster;

namespace Busniss.Services
{
    public class VoteService(AppDbContext context) : IVoteService
    {
        private readonly AppDbContext _context = context;

        public async Task<Result> AddAsync(int pollId, string userId, VoteRequest request, CancellationToken cancellationToken)
        {
            var hasVote = await _context.Votes.AnyAsync(x => x.PollId == pollId && x.UserId == userId, cancellationToken);
            if (hasVote) return Result.Failure(VoteErrors.DublicateVote);
            var pollIsExist = await _context.Polls.AnyAsync(
                                        p => p.Id == pollId
                                        && p.IsPublished
                                        && p.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow)
                                        && p.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow),
                                        cancellationToken
            );
            if (!pollIsExist) return Result.Failure(PolLErrors.NotFound);
            var availabeQuestions = await _context.Questions
                .Where(x => x.PollId == pollId && x.IsActive)
                .Select(x => x.Id)
                .ToListAsync(cancellationToken);
            if (!request.Answers.Select(x => x.QuestionId).SequenceEqual(availabeQuestions))
                return Result.Failure(VoteErrors.DublicateVote);
            var vote = new Vote
            {
                PollId = pollId,
                UserId = userId,
                VoteAnswers = request.Answers.Adapt<IEnumerable<VoteAnswer>>().ToList()
            };
            await _context.AddAsync(vote, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
