namespace Core.Abestraction.Errors;
public static class VoteErrors
{
    public static readonly Error NotFound = new Error("vote_not_found", "vote not found");
    public static readonly Error CreateFailed = new Error("vote_create_failed", "can't create this vote");
    public static readonly Error DublicateVote = new Error("vote_dublicate", "this vote is already exist");
}
