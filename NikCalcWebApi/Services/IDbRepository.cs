using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NikCalcWebApi.Models;
using NikCalcWebApi.Models.Requests;

namespace NikCalcWebApi.Services;

public interface IDbRepository
{
    Task AddLanguageAsync(LanguageRequest languageModel);
    Task AddReviewAsync(ReviewModel reviewModel);
    Task AddTabAsync(TabRequest tabModel);
    Task AddTextBlockAsync(TextBlockModel textBlockModel);
    Task<EntityEntry<UserModel>> AddUserAsync(UserModel user);
    Task<bool> CheckEmailTakenAsync(string email);
    Task<LanguageModel> GetLanguageAsync(string language);
    Task<List<string>> GetLanguagesNameAsync();
    Task<int> GetMaxPositionAsync();
    DbSet<ReviewModel> GetReviews();
    Task<TabModel> GetTabAsync(string tab);
    Task<List<string>> GetTabsNameAsync();
    Task<List<TextBlockModel>> GetTextsFromTabAsync(string tabName);
    Task<UserModel> GetUserByCredentialsAsync(string userName, string password);
    Task SaveChangesAsync();
    void UpdatePosition(TextBlockModel firstTextBlock, int newPosition);
    Task AddTextBlockToTab(TabModel tabModel, AddTextBlockRequest textBlockRequest);
    void UpdateTextBlock(TextBlockModel oldTextBlock, string newText);
}