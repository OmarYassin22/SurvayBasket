
using Core.Models;

namespace Core.Interfaces;
public interface IPollService
{

    public IEnumerable<Poll?> GetPolls();
    public Poll? GetPollById(int id);
    Poll CreatePool(Poll poll);
    bool Update(int id, Poll poll);
    bool Delete(int id);
}
