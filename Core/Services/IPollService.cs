﻿using Core.Abestraction;
using Core.Contracts.Poll;

namespace Core.Interfaces;
public interface IPollService
{

    Task<Result<IEnumerable<ResponsePoll?>>> GetPollsAsync(CancellationToken cancellationToken);
    Task<Result<IEnumerable<ResponsePoll>>> GetCurrentAsync(CancellationToken cancellationToken);
    Task<Result<Poll>> GetPollByIdAsync(int id, CancellationToken cancellationToken);
    Task<Result<Poll>> CreatePoolAsync(Poll poll, CancellationToken cancellation);
    Task<Result> UpdateAsync(int id, Poll request, CancellationToken cancellationToken);
    Task<Result> DeleteAsync(int id, CancellationToken cancellationToken);
    Task<Result<Poll>> TogglePublishAsync(int id, CancellationToken cancellationToken);
}
