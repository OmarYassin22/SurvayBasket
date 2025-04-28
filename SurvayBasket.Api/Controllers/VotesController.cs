using Core.Abestraction.Errors;
using Core.Contracts.Vote;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using SurvayBasket.Api.Extions;

namespace SurvayBasket.Api.Controllers
{
    [Route("api/polls/{pollId:int}/vote")]
    [ApiController]
    [Authorize]
    public class VotesController(IQuestionServices questionServices, IVoteService voteService) : ControllerBase
    {
        private readonly IQuestionServices _questionServices = questionServices;
        private readonly IVoteService _voteService = voteService;

        [HttpGet("")]
        public async Task<IActionResult> Start(int pollId, CancellationToken cancellationToken)
        {
            var userId = User.GetuserId();
            //if (userId is null) return BadRequest();
            var result = await _questionServices.GetAvailabeAsync(pollId, userId!, cancellationToken);
            if (result.IsSuccess) return Ok(result.Value);
            return result.Error.Equals(QuestionErrors.NotFound) ? NotFound(new { result.Error }) : BadRequest(new { result.Error });
        }
        [HttpPost("")]
        public async Task<IActionResult> Addasync([FromRoute] int pollId, [FromBody] VoteRequest request, CancellationToken cancellationToken)
        {
            var res = await _voteService.AddAsync(pollId, User.GetuserId()!, request, cancellationToken);
            if (res.IsSuccess) return Created();
            return res.Error.Equals(VoteErrors.NotFound) ? NotFound(new { res.Error }) : BadRequest(new { res.Error });
        }
    }
}
