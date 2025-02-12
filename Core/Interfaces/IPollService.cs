
using Core.Models;

namespace Core.Interfaces;
public interface IPollService
{

    Task<IEnumerable<Poll?>> GetPollsAsync(CancellationToken cancellationToken);
    Task<Poll?> GetPollByIdAsync(int id, CancellationToken cancellationToken);
    Task<Poll?> CreatePoolAsync(Poll poll, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(int id, Poll request, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
    Task<Poll?> TogglePublishAsync(int id, CancellationToken cancellationToken);
}
