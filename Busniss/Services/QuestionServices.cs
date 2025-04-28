using Busniss.Persistence;
using Core.Abestraction;
using Core.Abestraction.Errors;
using Core.Contracts.Answer;
using Core.Contracts.Question;
using Core.Services;
using Mapster;
using MapsterMapper;


namespace Busniss.Services
{
    public class QuestionServices(AppDbContext context, IMapper mapper) : IQuestionServices
    {
        private readonly AppDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest request, CancellationToken cancellationToken)
        {
            var pollIsExist = await _context.Polls.AnyAsync(p => p.Id == pollId, cancellationToken);
            if (!pollIsExist) return Result<QuestionResponse>.Failure(PolLErrors.NotFound);

            var questionIsExist = await _context.Questions.AnyAsync(q => q.Content == request.Content && q.PollId == pollId, cancellationToken);
            if (questionIsExist) return Result<QuestionResponse>.Failure(QuestionErrors.DublicateQuestionContect);

            var question = _mapper.Map<Question>(request);
            request.Answers.ForEach(answer => question.Answers.Add(new Answer { Content = answer }));
            question.PollId = pollId;

            await _context.AddAsync(question, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<QuestionResponse>.Success(question.Adapt<QuestionResponse>());
        }

        public async Task<Result<List<QuestionResponse>>> GetAllAsync(int pollId, CancellationToken cancellationToken)
        {
            try
            {
                var prds = await _context.Questions
                    .Where(q => q.PollId == pollId)
                    .Include(q => q.Answers)
                    //.Select(q => new QuestionResponse(
                    //    q.Id,
                    //    q.Content,
                    //    q.Answers.Select(a => new AnswerResponse(a.Id, a.Content)).ToList() // Fix: Convert IEnumerable to List
                    //))
                    .ProjectToType<QuestionResponse>()
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                var result = _mapper.Map<List<QuestionResponse>>(prds);
                return Result<List<QuestionResponse>>.Success(result);
            }
            catch (Exception)
            {
                return Result<List<QuestionResponse>>.Failure(QuestionErrors.NotFound);
            }
        }

        public async Task<Result<QuestionResponse>> GetByIdAsync(int pollId, int questionid, CancellationToken cancellationToken)
        {
            var question = await _context.Questions
                 .Where(q => q.PollId == pollId && q.Id == questionid)
                 .Include(q => q.Answers)
                 .ProjectToType<QuestionResponse>()
                 .AsNoTracking()
                 .FirstOrDefaultAsync(cancellationToken);
            if (question is null)
                return Result<QuestionResponse>.Failure(QuestionErrors.NotFound);

            var result = _mapper.Map<QuestionResponse>(question);
            return Result<QuestionResponse>.Success(result);
        }
        public async Task<Result> UpdateAsync(int pollId, int questionid, QuestionRequest request, CancellationToken cancellationToken)
        {
            var questionIsExist = await _context.Questions.AnyAsync
                                            (q => q.Id != questionid
                                            && q.PollId == pollId
                                            && q.Content == request.Content
                                            , cancellationToken);
            if (questionIsExist) return Result.Failure(QuestionErrors.DublicateQuestionContect);
            var question = await _context.Questions
                .Include(q => q.Answers)
                .SingleOrDefaultAsync(q => q.Id == questionid && q.PollId == pollId);
            if (question is null) return Result.Failure(QuestionErrors.NotFound);
            question.Content = request.Content;
            var currentAnswers = question.Answers.Select(cur => cur.Content).ToList();
            var newAnswers = request.Answers.Except(currentAnswers).ToList();
            newAnswers.ForEach(ans => { question.Answers.Add(new Answer { Content = ans }); });
            question.Answers.ToList().ForEach(answer =>
            {
                question.IsActive = request.Content.Contains(request.Content);

            });

            await _context.SaveChangesAsync();
            return Result.Success();

        }

        public async Task<Result> ToggleById(int pollId, int questionid, CancellationToken cancellationToken)
        {

            var question = await _context.Questions.SingleOrDefaultAsync(q => q.Id == questionid && q.PollId == pollId, cancellationToken);

            if (question is null)
                return Result<bool>.Failure(QuestionErrors.NotFound);

            question.IsActive = !question.IsActive;

            await _context.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }

        public async Task<Result<IEnumerable<QuestionResponse>>> GetAvailabeAsync(int pollId, string userId, CancellationToken cancellationToken)
        {
            var hasVote = await _context.Votes.AnyAsync(q => q.PollId == pollId && q.UserId == userId, cancellationToken);
            if (hasVote) return Result<IEnumerable<QuestionResponse>>.Failure(QuestionErrors.NotFound);
            var pollIsExist = await _context.Polls.AnyAsync(
                                        p => p.Id == pollId
                                        && p.IsPublished
                                        && p.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow)
                                        && p.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow),
                                        cancellationToken
            );
            if (!pollIsExist) return Result<IEnumerable<QuestionResponse>>.Failure(PolLErrors.NotFound);
            var questions = await _context.Questions
                .Where(q => q.IsActive && q.PollId == pollId)
                .Include(q => q.Answers)
                .Select(q =>
                    new QuestionResponse(
                        q.Id,
                        q.Content,
                   q.Answers.Where(a => a.IsActive).Select(a => new AnswerResponse(a.Id, a.Content)).ToList()))
                .AsNoTracking()
                .ToListAsync(cancellationToken);


            return Result<IEnumerable<QuestionResponse>>.Success(questions);
        }
    }
}
