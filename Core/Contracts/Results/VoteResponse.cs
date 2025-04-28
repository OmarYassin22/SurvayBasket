namespace Core.Contracts.Results
{
    record VoteResponse(
        string VoterName,
        DateTime VoteDate,
        IEnumerable<QuestionAnswerResponse> SelectedAnsers
    );
}
