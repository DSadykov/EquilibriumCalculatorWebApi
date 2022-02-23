using Microsoft.EntityFrameworkCore;
using NikCalcWebApi.DB;
using NikCalcWebApi.Models;
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
        await _mainDbContext.SaveChangesAsync();
    }
    public DbSet<ReviewModel> GetReviews()
    {
        return _mainDbContext.Reviews;
    }

    public async Task< List<TextBlockModel>> GetTextsFromTab(string tabName)
    {
        return await _mainDbContext.Texts
            .Where(x => x.Tab.TabName == tabName)
            .Include(x => x.Language)
            .ToListAsync();
    }

    public async Task<UserModel> GetUserByCredentials(string userName, string password)
    {
        return await _mainDbContext.Users.FirstOrDefaultAsync(x=>x.UserName== userName && x.Password==password);
    }

    public async Task<bool> CheckEmailTaken(string email)
    {
        var user= _mainDbContext.Users.FirstOrDefaultAsync(x => x.UserName == email);
        return (await user) != null;
    }

    public async Task AddUser(UserModel user)
    {
        await _mainDbContext.Users.AddAsync(user);
        await _mainDbContext.SaveChangesAsync();
    }

    public async Task<List<string>> GetTabsName()
    {
        return await _mainDbContext.Tabs.Select(x => x.TabName).ToListAsync();
    }

    public async Task<List<string>> GetLanguagesName()
    {
        return await _mainDbContext.Languages.Select(x => x.Name).ToListAsync();
    }
}



