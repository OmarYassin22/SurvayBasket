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
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var polls = await _pollService.GetPollsAsync(cancellationToken);
        var responsePoll = polls.Adapt<ResponsePoll>();

        return Ok(polls);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
    {
        var poll = await _pollService.GetPollByIdAsync(id, cancellationToken);
        if (poll == null)
        {
            return NotFound();
        }
        var conf = new TypeAdapterConfig();
        conf.NewConfig<Poll, ResponsePoll>()
            .Map(dist => dist.Notes, src => src.Summery);
        var res = poll.Adapt<ResponsePoll>(conf);
        return Ok(res);


    }
    [HttpPost()]
    public async Task<IActionResult> CreatePol(CreatePollRequest request, [FromServices] IValidator<CreatePollRequest> validator, CancellationToken cancellationToken)
    {

        var poll = await _pollService.CreatePoolAsync(request.Adapt<Poll>(), cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = poll.Id }, poll);

    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CreatePollRequest request, CancellationToken cancellationToken)
    {
        return await _pollService.UpdateAsync(id, request.Adapt<Poll>(), cancellationToken) ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        return await _pollService.DeleteAsync(id, cancellationToken) ? NoContent() : NotFound();
    }

    [HttpPost("{id}/toggle")]
    public async Task<IActionResult> TogglePublish(int id, CancellationToken cancellationToken)
    {
        var poll = await _pollService.TogglePublishAsync(id, cancellationToken);
        return poll == null ? NotFound() : Ok(poll);
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
