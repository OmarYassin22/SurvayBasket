namespace Core.Contracts.Question;
public class QuestionRequest
{
    public string Content { get; set; }
    public List<string> Answers { get; set; } = [];
}
