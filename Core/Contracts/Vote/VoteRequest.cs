namespace Core.Contracts.Vote
{
    public record VoteRequest(
          IEnumerable<VoteAnswerRequest> Answers
        );
}
