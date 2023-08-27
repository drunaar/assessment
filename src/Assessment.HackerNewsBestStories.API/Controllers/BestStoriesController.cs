using System.ComponentModel.DataAnnotations;

namespace Assessment.HackerNewsBestStories.API.Controllers;

[ApiController]
[Route("[controller]")]
public class BestStoriesController : ControllerBase {
    private readonly IMediator _mediator;
    private readonly ILogger<BestStoriesController> _logger;

    public BestStoriesController(IMediator mediator, ILogger<BestStoriesController> logger)
        => (_mediator, _logger) = (mediator, logger);

    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(StoryDTO[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<StoryDTO[]>> Get([FromQuery][Range(0, uint.MaxValue)] uint top = 100) {
        return Ok(await _mediator.Send(new GetBestStoriesQuery(top)));
    }
}
