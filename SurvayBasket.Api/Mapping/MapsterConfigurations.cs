using Core.Contracts.Poll;

namespace SurvayBasket.Api.Mapping;

public class MapsterConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Poll, ResponsePoll>().Map(dist => dist.Notes, src => src.Summery);
    }
}
