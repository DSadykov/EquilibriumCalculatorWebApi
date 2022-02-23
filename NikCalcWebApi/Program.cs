using ImageConverterWebApi.Services;
using Microsoft.EntityFrameworkCore;
using NikCalcWebApi.DB;
using NikCalcWebApi.Services;
using NikCalcWebApi.Services.Authenticate;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigureServices(builder.Services, builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
}
app.UseCors(builder => builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
#if DEBUG
app.Urls.Add("http://*:7777");
#endif
//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseMiddleware<JwtMiddleware>();

app.Run();
void ConfigureServices(IServiceCollection services, ConfigurationManager configuration)
{
    var connectionString = configuration.GetConnectionString("Default");
    services.AddDbContext<MainDbContext>(options => options.UseMySql(connectionString,
        new MySqlServerVersion(new Version(8, 0, 28))));
    services.AddCors();
    services.AddScoped<IDbRepository,DbRepository>();
    services.AddScoped<IUserService, UserService>();
    services.AddSingleton<EncryptionService>();
    services.AddTransient<JwtMiddleware>();
    services.AddControllers().AddNewtonsoftJson();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
}