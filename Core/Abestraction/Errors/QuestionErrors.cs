namespace Core.Abestraction.Errors;
public static class QuestionErrors
{
    public static readonly Error NotFound = new Error("question_not_found", "question not found");
    public static readonly Error CreateFailed = new Error("question_create_failed", "can't create this question");
    public static readonly Error DublicateQuestionContect = new Error("question_dublicate_content", "this question is already exist ");
}
