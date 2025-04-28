namespace Core.Contracts.Question;
public class QuestionRequest
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public List<string> Answers { get; set; } = [];
}
