using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NikCalcWebApi.DB;
using NikCalcWebApi.Models;
using NikCalcWebApi.Models.Requests;

namespace NikCalcWebApi.Services;

public class DbRepository : IDbRepository
{
    private readonly MainDbContext _mainDbContext;

    public DbRepository(MainDbContext mainDbContext)
    {
        _mainDbContext = mainDbContext;
    }
    public async Task AddReviewAsync(ReviewModel reviewModel)
    {
        await _mainDbContext.Reviews.AddAsync(reviewModel);
    }
    public DbSet<ReviewModel> GetReviews()
    {
        return _mainDbContext.Reviews;
    }

    public async Task<List<TextBlockModel>> GetTextsFromTabAsync(string tabName)
    {
        return await _mainDbContext.Texts
            .Where(x => x.Tab.TabName == tabName)
            .Include(x => x.Language)
            .ToListAsync();
    }

    public void UpdateTextBlock(TextBlockModel oldTextBlock, string newText)
    {
        oldTextBlock.Text = newText;
    }

    public async Task AddTabAsync(TabRequest tabModel)
    {
        await _mainDbContext.Tabs.AddAsync(new() { TabName = tabModel.TabName });
    }

    public async Task AddLanguageAsync(LanguageRequest languageModel)
    {
        await _mainDbContext.Languages.AddAsync(new() { Name = languageModel.Language });
    }

    public void UpdatePosition(TextBlockModel firstTextBlock, int newPosition)
    {
        firstTextBlock.Position = newPosition;
    }

    public async Task<int> GetMaxPositionAsync()
    {
        return await _mainDbContext.Texts.MaxAsync(x => x.Position);
    }

    public async Task<TabModel> GetTabAsync(string tab)
    {
        return await _mainDbContext.Tabs.FirstOrDefaultAsync(x => x.TabName == tab);
    }

    public async Task<LanguageModel> GetLanguageAsync(string language)
    {
        return await _mainDbContext.Languages.FirstOrDefaultAsync(x => x.Name == language);
    }

    public async Task AddTextBlockAsync(TextBlockModel textBlockModel)
    {
        await _mainDbContext.Texts.AddAsync(textBlockModel);
    }


    public async Task<UserModel> GetUserByCredentialsAsync(string userName, string password)
    {
        return await _mainDbContext.Users.FirstOrDefaultAsync(x => x.UserName == userName && x.Password == password);
    }

    public async Task<bool> CheckEmailTakenAsync(string email)
    {
        Task<UserModel?>? user = _mainDbContext.Users.FirstOrDefaultAsync(x => x.UserName == email);
        return (await user) != null;
    }

    public async Task<EntityEntry<UserModel>> AddUserAsync(UserModel user)
    {
        return await _mainDbContext.Users.AddAsync(user);
    }

    public async Task<List<string>> GetTabsNameAsync()
    {
        return await _mainDbContext.Tabs.Select(x => x.TabName).ToListAsync();
    }

    public async Task<List<string>> GetLanguagesNameAsync()
    {
        return await _mainDbContext.Languages.Select(x => x.Name).ToListAsync();
    }
    public async Task SaveChangesAsync()
    {
        await _mainDbContext.SaveChangesAsync();
    }

    public async Task AddTextBlockToTab(TabModel tabModel, AddTextBlockRequest textBlockRequest)
    {
        int position = (await GetMaxPositionAsync()) + 1;
        List<Task>? tasks = new(textBlockRequest.Texts.Count * 3);
        foreach (TextBlock? textBlock in textBlockRequest.Texts)
        {
            LanguageModel languageModel = await GetLanguageAsync(textBlock.Language);
            if (languageModel is null)
            {
                continue;
            }
            tasks.Add(AddTextBlockAsync(new TextBlockModel()
            {
                Text = textBlock.Text,
                LanguageId = languageModel.Id,
                TabId = tabModel.Id,
                Position = position
            }));
        }
        await Task.WhenAll(tasks);
        await SaveChangesAsync();
    }
}



