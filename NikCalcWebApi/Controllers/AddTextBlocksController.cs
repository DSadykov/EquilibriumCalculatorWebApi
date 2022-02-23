using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NikCalcWebApi.DB;
using NikCalcWebApi.Models;
using NikCalcWebApi.Requests;
using System.Linq;

namespace NikCalcWebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AddTextBlocksController : ControllerBase
{
    private readonly MainDbContext _reviewsDbContext;

    public AddTextBlocksController(MainDbContext reviewsDbContext)
    {
        _reviewsDbContext = reviewsDbContext;
    }
    [HttpPost("AddLanguage")]
    public async Task<ActionResult> Post([FromBody] LanguageRequest languageModel)
    {
        await _reviewsDbContext.Languages.AddAsync(new() { Name = languageModel.Language });
        _reviewsDbContext.SaveChanges();
        return Ok();
    }
    [HttpPost("AddTab")]
    public async Task<ActionResult> Post([FromBody] TabRequest tabModel)
    {
        await _reviewsDbContext.Tabs.AddAsync(new() { TabName = tabModel.TabName });
        _reviewsDbContext.SaveChanges();
        return Ok();
    }
    [HttpPost("AddTextBlockToTab")]
    public async Task<ActionResult> Post(AddTextBlockRequest textBlockRequest)
    {
        TabModel tabModel = _reviewsDbContext.Tabs.FirstOrDefault(x => x.TabName == textBlockRequest.Tab);
        if (tabModel is null)
        {
            return BadRequest("Specified tab does not exist!");
        }
        var position = _reviewsDbContext.Texts.Max(x => x.Position) + 1;
        foreach (var textBlock in textBlockRequest.Texts)
        {
            LanguageModel languageModel = _reviewsDbContext.Languages.FirstOrDefault(x => x.Name == textBlock.Language);
            if (languageModel is null)
            {
                continue;
            }
            await _reviewsDbContext.Texts.AddAsync(new()
            {
                Text = textBlock.Text,
                LanguageId = languageModel.Id,
                TabId = tabModel.Id,
                Position = position
            });
        }
        _reviewsDbContext.SaveChanges();
        return Ok();
    }
    

}
