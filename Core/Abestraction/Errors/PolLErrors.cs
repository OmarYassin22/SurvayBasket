namespace Core.Abestraction.Errors;
public static class PolLErrors
{
    public static readonly Error NotFound = new Error("poll_not_found", "Poll not found");
    public static readonly Error CreateFailed = new Error("poll_create_failed", "can't create this poll");
    public static readonly Error DublicatePoll = new Error("poll_dublicate", "this poll is already exist");
}
