using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NikCalcWebApi.Models;
using NikCalcWebApi.Responses;
using NikCalcWebApi.Services;

namespace NikCalcWebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ReviewsController : ControllerBase
{
    private readonly ILogger<ReviewsController> _logger;
    private readonly DbRepository _usersDb;

    public ReviewsController(ILogger<ReviewsController> logger, DbRepository usersDb)
    {
        _logger = logger;
        _usersDb = usersDb;
    }
    [HttpGet("GetAllReviews")]
    public async Task<ActionResult> Get()
    {
        var serializedResult = Task.Run(() =>
        {
            return JsonConvert.SerializeObject(_usersDb
                .GetReviews()
                .Select(x => new ReviewResponse()
            {
                Review = x.Review
            }));
        });
        return Content(await serializedResult);
    }
    [HttpPost("AddReview")]
    public async Task<ActionResult> Post([FromBody] ReviewModel review)
    {
        await _usersDb.AddReview(review);
        return Ok();
    }
}
