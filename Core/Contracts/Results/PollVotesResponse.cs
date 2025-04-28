namespace Core.Contracts.Results
{
    record PollVotesResponse
  (
      string Title,
      IEnumerable<VoteResponse> Votes);
}
