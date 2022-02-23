using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NikCalcWebApi.Services;

namespace NikCalcWebApi.Controllers;

[ApiController, Route("api/[controller]")]
public class GetTextBlocksController : ControllerBase
{
    private readonly IDbRepository _dbRepository;

    public GetTextBlocksController(IDbRepository reviewsDbContext)
    {
        _dbRepository = reviewsDbContext;
    }
    [HttpGet("GetTextBlocks")]
    public async Task<ActionResult> Get(string tabName)
    {
        List<Models.TextBlockModel>? texts = await _dbRepository.GetTextsFromTabAsync(tabName);
        IEnumerable<IGrouping<int, Models.TextBlockModel>>? groupedTexts = texts.GroupBy(x => x.Position);
        List<Dictionary<string, string>>? result = new List<Dictionary<string, string>>(groupedTexts.Count());
        int tmpPos = 0;
        foreach (IGrouping<int, Models.TextBlockModel>? groupedText in groupedTexts)
        {
            result.Add(new());
            foreach (Models.TextBlockModel? value in groupedText)
            {
                result[tmpPos][value.Language.Name] = value.Text;
            }
            tmpPos++;
        }
        string? serializedResult = JsonConvert.SerializeObject(result);
        return Content(serializedResult);
    }
    [HttpGet("GetAllTabs")]
    public async Task<ActionResult> GetTabs()
    {
        Task<List<string>>? result = _dbRepository.GetTabsNameAsync();
        string? serializedResult = JsonConvert.SerializeObject(await result);
        return Content(serializedResult);
    }
    [HttpGet("GetAllLanguages")]
    public async Task<ActionResult> GetLanguages()
    {
        Task<List<string>>? result = _dbRepository.GetLanguagesNameAsync();
        string? serializedResult = JsonConvert.SerializeObject(await result);
        return Content(serializedResult);
    }
}