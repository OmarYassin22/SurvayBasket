namespace Core.Contracts.Poll;

public record CreatePollRequest(
    string? Title,
    string Summery,

    bool IsPublished,
    DateOnly StartsAt,
    DateOnly EndsAt

    );
