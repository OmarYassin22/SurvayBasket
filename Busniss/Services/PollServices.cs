using Busniss.Persistence;
using Core.Abestraction;
using Core.Abestraction.Errors;
using Core.Interfaces;

namespace Busniss.Services;
public class PollServices(AppDbContext context) : IPollService
{
    private AppDbContext _context { get; } = context;


    public async Task<Result<IEnumerable<Poll?>>> GetPollsAsync(CancellationToken cancellationToken)
    {

        var result = await _context.Polls.AsNoTracking().ToListAsync();

        return result is null
            ? Result<IEnumerable<Poll?>>.Failure(PolLErrors.NotFound)
            : Result<IEnumerable<Poll?>>.Success(result);

    }
    public async Task<Result<Poll>> GetPollByIdAsync(int id, CancellationToken cancellationToken)
    {
        var result = await _context.Polls.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        return result is null
            ? Result<Poll>.Failure(PolLErrors.NotFound)
            : Result<Poll>.Success(result);
    }
    public async Task<Result<Poll>> CreatePoolAsync(Poll poll, CancellationToken cancellation)
    {
        var isExits = await _context.Polls.AnyAsync(p => p.Title == poll.Title, cancellation);
        if (isExits)
            return Result<Poll>.Failure(PolLErrors.DublicatePoll);

        if (poll is null)
            return Result<Poll>.Failure(PolLErrors.CreateFailed);
        var r = await _context.AddAsync(poll, cancellation);
        await _context.SaveChangesAsync(cancellation);
        return Result<Poll>.Success(poll);
    }
    public async Task<Result> UpdateAsync(int id, Poll request, CancellationToken cancellationToken)
    {
        var isExist = await _context.Polls.AnyAsync(p => p.Title == request.Title && p.Id != request.Id, cancellationToken);
        if (isExist)
            return Result.Failure(PolLErrors.DublicatePoll);
        var poll = await _context.Polls.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        if (poll is null)
            return Result.Failure(PolLErrors.NotFound);
        poll.Title = request.Title;
        poll.Summery = request.Summery;
        poll.IsPublished = request.IsPublished;
        poll.StartsAt = request.StartsAt;
        poll.EndsAt = request.EndsAt;
        //_context.Polls.Update(lue);
        await _context.SaveChangesAsync();
        return Result.Success();

    }
    public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var isDeleted = await _context.Polls.FindAsync(id, cancellationToken);
        if (isDeleted is null)
            return Result.Failure(PolLErrors.NotFound);
        var r = _context.Remove(isDeleted);
        _context.SaveChanges();
        return Result.Success();
    }



    public async Task<Result<Poll>> TogglePublishAsync(int id, CancellationToken cancellationToken)
    {
        var result = await GetPollByIdAsync(id, cancellationToken);
        if (result.IsFailure)
            return Result<Poll>.Failure(result.Error);
        result.Value.IsPublished = !result.Value.IsPublished;
        _context.Polls.Update(result.Value);
        await _context.SaveChangesAsync();
        return Result<Poll>.Success(result.Value);

    }

}
