using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NikCalcWebApi.DB;
using NikCalcWebApi.Services;

namespace NikCalcWebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UpdateTextBlocksController : ControllerBase
{
    private readonly DbRepository _reviewsDbContext;

    public UpdateTextBlocksController(DbRepository reviewsDbContext)
    {
        _reviewsDbContext = reviewsDbContext;
    }
    [HttpPut("ChangeText")]
    public async Task<ActionResult> Put(string oldText, string newText, string tabName)
    {
        var textsFromTab = await _reviewsDbContext.GetTextsFromTab(tabName);
        var oldTextBlock = textsFromTab.FirstOrDefault(x => x.Text == oldText);
        if (oldTextBlock == null)
        {
            return BadRequest("Specified text does not exist!");
        }
        await _reviewsDbContext.UpdateTextBlock(oldTextBlock, newText);
        await _reviewsDbContext.SaveChangesAsync();
        return Ok();
    }
    [HttpPut("SwapPositions")]
    public async Task<ActionResult> Put(string tabName, int oldPosition, int newPosition)
    {
        var textsOnRequieredTab = await _reviewsDbContext.GetTextsFromTab(tabName);
        var firstTextBlock = textsOnRequieredTab.FirstOrDefault(x => x.Position == oldPosition);
        if (firstTextBlock == null)
        {
            return BadRequest("Specified poistion does not exist!"); 
        }
        var secondTextBlock = textsOnRequieredTab.FirstOrDefault(x => x.Position == newPosition);
        var tasks = new List<Task>
        {
            _reviewsDbContext.UpdatePosition(firstTextBlock, newPosition)
        };
        if (secondTextBlock is not null)
        {
            tasks.Add(_reviewsDbContext.UpdatePosition(secondTextBlock, oldPosition));
        }
        await Task.WhenAll(tasks);
        await _reviewsDbContext.SaveChangesAsync();
        return Ok();
    }
}
