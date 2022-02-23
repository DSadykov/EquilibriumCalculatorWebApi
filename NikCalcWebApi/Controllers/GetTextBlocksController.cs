using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NikCalcWebApi.DB;
using NikCalcWebApi.Services;

namespace NikCalcWebApi.Controllers;

[ApiController, Route("api/[controller]")]
public class GetTextBlocksController : ControllerBase
{
    private DbRepository _dbRepository;

    public GetTextBlocksController(DbRepository reviewsDbContext)
    {
        _dbRepository = reviewsDbContext;
    }
    [HttpGet("GetTextBlocks")]
    public async Task<ActionResult> Get(string tabName)
    {
        var texts = await _dbRepository.GetTextsFromTab(tabName);
        var groupedTexts = texts.GroupBy(x => x.Position);
        var result = new List<Dictionary<string, string>>(groupedTexts.Count());
        var tmpPos = 0;
        foreach (var groupedText in groupedTexts)
        {
            result.Add(new());
            foreach (var value in groupedText)
            {
                result[tmpPos][value.Language.Name] = value.Text;
            }
            tmpPos++;
        }
        var serializedResult = JsonConvert.SerializeObject(result);
        return Content(serializedResult);
    }
    [HttpGet("GetAllTabs")]
    public async Task<ActionResult> GetTabs()
    {
        var result = _dbRepository.GetTabsName();
        var serializedResult = JsonConvert.SerializeObject(await result);
        return Content(serializedResult);
    }
    [HttpGet("GetAllLanguages")]
    public async Task<ActionResult> GetLanguages()
    {
        var result = _dbRepository.GetLanguagesName();
        var serializedResult = JsonConvert.SerializeObject(await result);
        return Content(serializedResult);
    }
}