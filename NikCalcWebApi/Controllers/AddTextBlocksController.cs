using Microsoft.AspNetCore.Mvc;
using NikCalcWebApi.Models;
using NikCalcWebApi.Models.Requests;
using NikCalcWebApi.Services;

namespace NikCalcWebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AddTextBlocksController : ControllerBase
{
    private readonly IDbRepository _dbRepository;

    public AddTextBlocksController(IDbRepository reviewsDbContext)
    {
        _dbRepository = reviewsDbContext;
    }
    [HttpPost("AddLanguage")]
    public async Task<ActionResult> Post([FromBody] LanguageRequest languageModel)
    {
        await _dbRepository.AddLanguageAsync(languageModel);
        await _dbRepository.SaveChangesAsync();
        return Ok();
    }
    [HttpPost("AddTab")]
    public async Task<ActionResult> Post([FromBody] TabRequest tabModel)
    {
        await _dbRepository.AddTabAsync(tabModel);
        await _dbRepository.SaveChangesAsync();
        return Ok();
    }
    [HttpPost("AddTextBlockToTab")]
    public async Task<ActionResult> Post(AddTextBlockRequest textBlockRequest)
    {
        TabModel tabModel = await _dbRepository.GetTabAsync(textBlockRequest.Tab);
        if (tabModel is null)
        {
            return BadRequest("Specified tab does not exist!");
        }
        await _dbRepository.AddTextBlockToTab(tabModel,textBlockRequest);
        return Ok();
    }
}
