

using Core.Contracts.Response;
using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SurvayBasket.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PollsController(IPollService pollService, IMapper mapper) : ControllerBase
{
    private readonly IPollService _pollService = pollService;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    public IActionResult GetAll()
    {
        var polls = _pollService.GetPolls();
        return Ok(polls as ResponsePoll);
    }
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var poll = _pollService.GetPollById(id);
        if (poll == null)
        {
            return NotFound();
        }
        var conf = new TypeAdapterConfig();
        conf.NewConfig<Poll, ResponsePoll>()
            .Map(dist => dist.Notes, src => src.Description);
        var res = poll.Adapt<ResponsePoll>(conf);
        return Ok(res);


    }
    [HttpPost()]
    public IActionResult CreatePol(CreatePoll request, [FromServices] IValidator<CreatePoll> validator)
    {
        //var validationResult = validator.Validate(request);
        //if (!validationResult.IsValid)
        //{
        //    var erros = new ModelStateDictionary();


        //    validationResult.Errors.ForEach(x => erros.AddModelError(x.ErrorCode, x.ErrorMessage));

        //    return ValidationProblem(erros);
        //}
        var poll = _pollService.CreatePool(request.Adapt<Poll>());
        return CreatedAtAction(nameof(Get), new { id = poll.Id }, poll);

    }
    [HttpPut("{id}")]
    public IActionResult Update(int id, CreatePoll request)
    {
        //return _pollService.Update(id, (Poll)request) ? NoContent() : NotFound();
        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        return _pollService.Delete(id) ? NoContent() : NotFound();
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
