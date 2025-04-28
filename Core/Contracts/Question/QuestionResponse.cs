using Core.Contracts.Answer;

namespace Core.Contracts.Question;
public record QuestionResponse
(
    int Id,
    string Content,
    List<AnswerResponse> Answers
    );