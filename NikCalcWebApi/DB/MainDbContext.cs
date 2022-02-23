using Microsoft.EntityFrameworkCore;
using NikCalcWebApi.Models;

namespace NikCalcWebApi.DB;

public class MainDbContext : DbContext
{
    public MainDbContext(DbContextOptions<MainDbContext> options)
            : base(options) { }
    public DbSet<ReviewModel> Reviews { get; set; }
    public DbSet<TabModel> Tabs { get; set; }
    public DbSet<LanguageModel> Languages { get; set; }
    public DbSet<TextBlockModel> Texts { get; set; }
    public DbSet<UserModel> Users { get; set; }
}
