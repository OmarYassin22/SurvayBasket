global using Core.Interfaces;
using Busniss.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Busniss.Services;
public class PollServices(AppDbContext context) : IPollService
{
    private AppDbContext _context { get; } = context;

    public async Task<Poll?> CreatePoolAsync(Poll poll, CancellationToken cancellation)
    {

        if (poll is null)
            return null;
        var r = await _context.AddAsync(poll, cancellation);
        await _context.SaveChangesAsync();
        return poll;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var isDeleted = await _context.Polls.FindAsync(id, cancellationToken);
        if (isDeleted is null)
            return false;
        _context.Remove(isDeleted);
        return true;
    }

    public async Task<Poll?> GetPollByIdAsync(int id, CancellationToken cancellationToken)
    => await _context.Polls.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public async Task<IEnumerable<Poll?>> GetPollsAsync(CancellationToken cancellationToken)
    => await _context.Polls.AsNoTracking().ToListAsync();

    public async Task<bool> UpdateAsync(int id, Poll request, CancellationToken cancellationToken)
    {
        var currentPoll = await GetPollByIdAsync(id, cancellationToken);
        if (currentPoll is null)

            return false;
        currentPoll.Title = request.Title;
        currentPoll.Summery = request.Summery;
        currentPoll.IsPublished = request.IsPublished;
        currentPoll.StartsAt = request.StartsAt;
        currentPoll.EndsAt = request.EndsAt;
        _context.Polls.Update(currentPoll);
        await _context.SaveChangesAsync();

        return true;
    }
    public async Task<Poll?> TogglePublishAsync(int id, CancellationToken cancellationToken)
    {
        var poll = await GetPollByIdAsync(id, cancellationToken);
        if (poll is null)
            return null;
        poll.IsPublished = !poll.IsPublished;
        _context.Polls.Update(poll);
        await _context.SaveChangesAsync();
        return poll;

    }

}
