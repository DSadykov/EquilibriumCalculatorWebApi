using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NikCalcWebApi.Models;
using NikCalcWebApi.Models.Responses;
using NikCalcWebApi.Services;

namespace NikCalcWebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ReviewsController : ControllerBase
{
    private readonly IDbRepository _usersDb;

    public ReviewsController(IDbRepository usersDb)
    {
        _usersDb = usersDb;
    }
    [HttpGet("GetAllReviews")]
    public async Task<ActionResult> Get()
    {
        Task<string>? serializedResult = Task.Run(() =>
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
        await _usersDb.AddReviewAsync(review);
        return Ok();
    }
}
