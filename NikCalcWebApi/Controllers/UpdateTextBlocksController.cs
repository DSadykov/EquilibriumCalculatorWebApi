using Microsoft.AspNetCore.Mvc;
using NikCalcWebApi.Models;
using NikCalcWebApi.Services;

namespace NikCalcWebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UpdateTextBlocksController : ControllerBase
{
    private readonly IDbRepository _reviewsDbContext;

    public UpdateTextBlocksController(IDbRepository reviewsDbContext)
    {
        _reviewsDbContext = reviewsDbContext;
    }
    [HttpPut("ChangeText")]
    public async Task<ActionResult> Put(string oldText, string newText, string tabName)
    {
        List<TextBlockModel>? textsFromTab = await _reviewsDbContext.GetTextsFromTabAsync(tabName);
        TextBlockModel? oldTextBlock = textsFromTab.FirstOrDefault(x => x.Text == oldText);
        if (oldTextBlock == null)
        {
            return BadRequest("Specified text does not exist!");
        }
        _reviewsDbContext.UpdateTextBlock(oldTextBlock, newText);
        await _reviewsDbContext.SaveChangesAsync();
        return Ok();
    }
    [HttpPut("SwapPositions")]
    public async Task<ActionResult> Put(string tabName, int oldPosition, int newPosition)
    {
        List<TextBlockModel>? textsOnRequieredTab = await _reviewsDbContext.GetTextsFromTabAsync(tabName);
        TextBlockModel? firstTextBlock = textsOnRequieredTab.FirstOrDefault(x => x.Position == oldPosition);
        if (firstTextBlock == null)
        {
            return BadRequest("Specified poistion does not exist!");
        }
        TextBlockModel? secondTextBlock = textsOnRequieredTab.FirstOrDefault(x => x.Position == newPosition);
        _reviewsDbContext.UpdatePosition(firstTextBlock, newPosition);
        if (secondTextBlock is not null)
        {
            _reviewsDbContext.UpdatePosition(secondTextBlock, oldPosition);
        }
        await _reviewsDbContext.SaveChangesAsync();
        return Ok();
    }
}
