global using Core.Interfaces;
using Core.Models;

namespace Busniss;
public class PollServices : IPollService
{
    private static readonly List<Poll> _polls = [
        new Poll { Id = 1, Title = "Poll 1", Description = "Description 1" },
        new Poll { Id = 2, Title = "Poll 2", Description = "Description 2" },
        new Poll { Id = 3, Title = "Poll 3", Description = "Description 3" },
        ];

    public Poll CreatePool(Poll poll)
    {
        poll.Id = _polls.Count + 1;
        _polls.Add(poll);
        return poll;
    }

    public bool Delete(int id)
    {
        var isDeleted = GetPollById(id);
        if (isDeleted is null)
            return false;
        _polls.Remove(isDeleted);
        return true;
    }

    public Poll? GetPollById(int id)
    => _polls.FirstOrDefault(p => p.Id == id);

    public IEnumerable<Poll> GetPolls()
    => _polls;
    public bool Update(int id, Poll poll)
    {
        var currentPoll = GetPollById(id);
        if (currentPoll is null)

            return false;
        currentPoll.Title = poll.Title;
        currentPoll.Description = poll.Description;
        return true;

    }
}
