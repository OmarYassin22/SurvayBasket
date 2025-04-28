using Core.Abestraction;
using Core.Contracts.Question;

namespace Core.Services
{
    public interface IQuestionServices
    {
        Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest question, CancellationToken cancellationToken);
        Task<Result<IEnumerable<QuestionResponse>>> GetAvailabeAsync(int pollId, string userId, CancellationToken cancellationToken); Task<Result<List<QuestionResponse>>> GetAllAsync(int pollId, CancellationToken cancellationToken);
        Task<Result<QuestionResponse>> GetByIdAsync(int pollId, int questionid, CancellationToken cancellationToken);
        Task<Result> ToggleById(int pollId, int questionid, CancellationToken cancellationToken);
        Task<Result> UpdateAsync(int pollId, int questionid, QuestionRequest request, CancellationToken cancellationToken);
    }
}
