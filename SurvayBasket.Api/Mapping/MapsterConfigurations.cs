using Core.Contracts.Poll;
using Core.Contracts.Question;

namespace SurvayBasket.Api.Mapping;

public class MapsterConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Poll, ResponsePoll>().Map(dist => dist.summery, src => src.Summery);
        config.NewConfig<QuestionRequest, Question>().Ignore(dist => dist.Answers);

        config.NewConfig<Question, QuestionResponse>()
    .Map(dest => dest.Answers, src => src.Answers.Select(a => a.Content).ToList());

    }
}
