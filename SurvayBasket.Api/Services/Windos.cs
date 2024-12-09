using SurvayBasket.Api.Interfaces;

namespace SurvayBasket.Api.Services;

public class Windos : IOS
{
    public Guid MyGuid { get ; set; }

    public string Run()
    {
      return MyGuid.ToString()[^4..];
    }
}
