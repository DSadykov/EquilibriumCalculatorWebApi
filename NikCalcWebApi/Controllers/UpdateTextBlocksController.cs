using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NikCalcWebApi.DB;

namespace NikCalcWebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UpdateTextBlocksController : ControllerBase
{
    private readonly MainDbContext _reviewsDbContext;

    public UpdateTextBlocksController(MainDbContext reviewsDbContext)
    {
        _reviewsDbContext = reviewsDbContext;
    }
    [HttpPut("ChangeText")]
    public async Task<ActionResult> Put(string oldText, string newText, string tabName)
    {
        var oldTextBlock = await _reviewsDbContext.Texts.Where(x => x.Tab.TabName == tabName)
                                                        .FirstOrDefaultAsync(x => x.Text == oldText);
        if (oldTextBlock == null)
        {
            return BadRequest("Specified text does not exist!");
        }
        oldTextBlock.Text = newText;
        await _reviewsDbContext.SaveChangesAsync();
        return Ok();
    }
    [HttpPut("SwapPositions")]
    public async Task<ActionResult> Put(string tabName, int oldPosition, int newPosition)
    {
        var textsOnRequiereTab = _reviewsDbContext.Texts.Where(x => x.Tab.TabName == tabName);
        var firstTextBlock = await textsOnRequiereTab.FirstOrDefaultAsync(x => x.Position == oldPosition);
        var secondTextBlock = await textsOnRequiereTab.FirstOrDefaultAsync(x => x.Position == newPosition);
        if (firstTextBlock == null)

        {
            return BadRequest("Specified poistion does not exist!");
        }
        firstTextBlock.Position = newPosition;
        if (secondTextBlock is not null)
        {
            secondTextBlock.Position = oldPosition;
        }
        await _reviewsDbContext.SaveChangesAsync();
        return Ok();
    }
}
