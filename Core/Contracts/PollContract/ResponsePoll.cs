namespace Core.Contracts.Poll;

public record ResponsePoll(int Id, string Title, string summery,
    bool isPublished, DateOnly StartsAt, DateOnly EndsAt
    );