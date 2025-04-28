using Core.Abestraction.Errors;
using Core.Contracts.Question;
using Core.Services;
using Microsoft.AspNetCore.Authorization;

namespace SurvayBasket.Api.Controllers
{
    [Route("api/polls/{pollId:int}/[controller]")]
    [ApiController]
    [Authorize]
    public class QuestionsController(IQuestionServices questionServices) : ControllerBase
    {
        private readonly IQuestionServices _questionServices = questionServices;
        [HttpGet("")]
        public async Task<IActionResult> Get([FromRoute] int pollId, CancellationToken cancellationToken)
        {
            var result = await _questionServices.GetAllAsync(pollId, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpGet("{questionId:int}")]
        public async Task<IActionResult> GetById([FromRoute] int pollId, [FromRoute] int questionId, CancellationToken cancellationToken)
        {

            var result = await _questionServices.GetByIdAsync(pollId, questionId, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(new { result.Error });
        }

        [HttpPut("{questionId:int}/toggle-status")]
        public async Task<IActionResult> ToggleById([FromRoute] int pollId, [FromRoute] int questionId, CancellationToken cancellationToken)
        {

            var result = await _questionServices.ToggleById(pollId, questionId, cancellationToken);

            return result.IsSuccess ? Ok() : BadRequest(new { result.Error });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateQuetion([FromRoute] int pollId, [FromRoute] int id, [FromBody] QuestionRequest request, CancellationToken cancellationToken)
        {
            var result = await _questionServices.UpdateAsync(pollId, id, request, cancellationToken);
            return result.IsSuccess ? NoContent() : BadRequest(new { result.Error });
        }
        [HttpPost]
        public async Task<IActionResult> CreateQuestion([FromRoute] int pollId, [FromBody] QuestionRequest request, CancellationToken cancellationToken)
        {
            var result = await _questionServices.AddAsync(pollId, request, cancellationToken);
            if (result.IsSuccess) return CreatedAtAction(nameof(Get), new { pollId, result.Value.Id }, result.Value);

            return result.Error.Equals(QuestionErrors.DublicateQuestionContect)
                ? Problem(statusCode: StatusCodes.Status409Conflict, title: result.Error.code, detail: result.Error.description)
                : Problem(statusCode: StatusCodes.Status404NotFound, title: result.Error.code, detail: result.Error.description);

        }
    }
}
