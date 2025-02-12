namespace Core.Contracts.Poll;

public record ResponsePoll(int Id, string Title, string Notes,
    bool IsPublished, DateOnly StartsAt, DateOnly EndsAt
    );