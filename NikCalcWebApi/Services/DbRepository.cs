using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using NikCalcWebApi.DB;
using NikCalcWebApi.Models;
using NikCalcWebApi.Requests;
using System;

namespace NikCalcWebApi.Services;

public class DbRepository
{
    private readonly MainDbContext _mainDbContext;

    public DbRepository(MainDbContext mainDbContext)
    {
        _mainDbContext = mainDbContext;
    }
    public async Task AddReview(ReviewModel reviewModel)
    {
        await _mainDbContext.Reviews.AddAsync(reviewModel);
    }
    public DbSet<ReviewModel> GetReviews()
    {
        return _mainDbContext.Reviews;
    }

    public async Task<List<TextBlockModel>> GetTextsFromTab(string tabName)
    {
        return await _mainDbContext.Texts
            .Where(x => x.Tab.TabName == tabName)
            .Include(x => x.Language)
            .ToListAsync();
    }

    internal async Task UpdateTextBlock(TextBlockModel oldTextBlock, string newText)
    {
        oldTextBlock.Text = newText;
    }

    public async Task AddTab(TabRequest tabModel)
    {
        await _mainDbContext.Tabs.AddAsync(new() { TabName = tabModel.TabName });
    }

    public async Task AddLanguage(LanguageRequest languageModel)
    {
        await _mainDbContext.Languages.AddAsync(new() { Name = languageModel.Language });
    }

    public async Task UpdatePosition(TextBlockModel firstTextBlock, int newPosition)
    {
        firstTextBlock.Position = newPosition;
    }

    public async Task<int> GetMaxPosition()
    {
        return await _mainDbContext.Texts.MaxAsync(x => x.Position);
    }

    public async Task<TabModel> GetTab(string tab)
    {
        return await _mainDbContext.Tabs.FirstOrDefaultAsync(x => x.TabName == tab);
    }

    public async Task<LanguageModel> GetLanguage(string language)
    {
        return await _mainDbContext.Languages.FirstOrDefaultAsync(x => x.Name == language);
    }

    public async Task AddTextBlock(TextBlockModel textBlockModel)
    {
        await _mainDbContext.Texts.AddAsync(textBlockModel);
    }


    public async Task<UserModel> GetUserByCredentials(string userName, string password)
    {
        return await _mainDbContext.Users.FirstOrDefaultAsync(x => x.UserName == userName && x.Password == password);
    }

    public async Task<bool> CheckEmailTaken(string email)
    {
        var user = _mainDbContext.Users.FirstOrDefaultAsync(x => x.UserName == email);
        return (await user) != null;
    }

    public async Task AddUser(UserModel user)
    {
        await _mainDbContext.Users.AddAsync(user);
        
    }

    public async Task<List<string>> GetTabsName()
    {
        return await _mainDbContext.Tabs.Select(x => x.TabName).ToListAsync();
    }

    public async Task<List<string>> GetLanguagesName()
    {
        return await _mainDbContext.Languages.Select(x => x.Name).ToListAsync();
    }
    public async Task SaveChangesAsync()
    {
        await _mainDbContext.SaveChangesAsync();
    }
}



