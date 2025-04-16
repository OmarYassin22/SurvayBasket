using Core.Contracts.Poll;
using Microsoft.AspNetCore.Authorization;

namespace SurvayBasket.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PollsController(IPollService pollService, IMapper mapper) : ControllerBase
{
    private readonly IPollService _pollService = pollService;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    [ResponseCache(Duration =60)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var polls = await _pollService.GetPollsAsync(cancellationToken);
        return polls.IsSuccess
             ? Ok(polls.Value)
              : BadRequest(polls.Error);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
    {
        var result = await _pollService.GetPollByIdAsync(id, cancellationToken);
        if (result.IsFailure)
        {
            return Problem(statusCode: StatusCodes.Status404NotFound, title: result.Error.code, detail: result.Error.description);
        }
        var conf = new TypeAdapterConfig();
        //conf.NewConfig<Poll, ResponsePoll>()
        //    .Map(dist => dist.summery, src => src.Summery);
        var res = result.Value.Adapt<ResponsePoll>();
        return Ok(res);


    }
    [HttpPost()]
    public async Task<IActionResult> CreatePol(CreatePollRequest request, [FromServices] IValidator<CreatePollRequest> validator, CancellationToken cancellationToken)
    {
        if (validator is null)
        {
            throw new ArgumentNullException(nameof(validator));
        }

        var result = await _pollService.CreatePoolAsync(request.Adapt<Poll>(), cancellationToken);
        return result.IsFailure ?
            Problem(statusCode: StatusCodes.Status409Conflict, title: result.Error.code, detail: result.Error.description)
            : CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value.Adapt<ResponsePoll>());


    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CreatePollRequest request, CancellationToken cancellationToken)
    {
        var result = await _pollService.UpdateAsync(id, request.Adapt<Poll>(), cancellationToken);

        return result.IsSuccess ? NoContent() : Problem(statusCode: StatusCodes.Status409Conflict, title: result.Error.code, detail: result.Error.description);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _pollService.DeleteAsync(id, cancellationToken);
        return result.IsSuccess ? NoContent() : Problem(statusCode: StatusCodes.Status404NotFound, title: result.Error.code, detail: result.Error.description);
    }

    [HttpPost("{id}/toggle")]
    public async Task<IActionResult> TogglePublish(int id, CancellationToken cancellationToken)
    {
        var poll = await _pollService.TogglePublishAsync(id, cancellationToken);
        return poll == null ? Problem(statusCode: StatusCodes.Status404NotFound, title: poll?.Error.code, detail: poll?.Error.description) : Ok(poll.Value);
    }
    [HttpPost("test")]
    public IActionResult Test([FromBody] Student student)
    {

        return Ok("Accepted Values");
    }
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
