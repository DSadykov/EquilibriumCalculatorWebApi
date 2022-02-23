using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NikCalcWebApi.DB;
using NikCalcWebApi.Models;
using NikCalcWebApi.Requests;
using NikCalcWebApi.Services;
using System.Linq;

namespace NikCalcWebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AddTextBlocksController : ControllerBase
{
    private readonly DbRepository _dbRepository;

    public AddTextBlocksController(DbRepository reviewsDbContext)
    {
        _dbRepository = reviewsDbContext;
    }
    [HttpPost("AddLanguage")]
    public async Task<ActionResult> Post([FromBody] LanguageRequest languageModel)
    {
        await _dbRepository.AddLanguage(languageModel);
        await _dbRepository.SaveChangesAsync();
        return Ok();
    }
    [HttpPost("AddTab")]
    public async Task<ActionResult> Post([FromBody] TabRequest tabModel)
    {
        await _dbRepository.AddTab(tabModel);
        await _dbRepository.SaveChangesAsync();
        return Ok();
    }
    [HttpPost("AddTextBlockToTab")]
    public async Task<ActionResult> Post(AddTextBlockRequest textBlockRequest)
    {
        TabModel tabModel = await _dbRepository.GetTab(textBlockRequest.Tab);
        if (tabModel is null)
        {
            return BadRequest("Specified tab does not exist!");
        }
        var position = (await _dbRepository.GetMaxPosition()) + 1;
        var tasks = new List<Task>();
        foreach (var textBlock in textBlockRequest.Texts)
        {
            LanguageModel languageModel = await _dbRepository.GetLanguage(textBlock.Language);
            if (languageModel is null)
            {
                continue;
            }
            tasks.Add( _dbRepository.AddTextBlock(new TextBlockModel()
            {
                Text = textBlock.Text,
                LanguageId = languageModel.Id,
                TabId = tabModel.Id,
                Position = position
            }));
        }
        await Task.WhenAll(tasks);
        await _dbRepository.SaveChangesAsync();
        return Ok();
    }
    

}
