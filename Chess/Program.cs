using Chess.Abstractions.Services;
using Chess.Infrastructure;
using Chess.Intefaces.Infrastructure;
using Chess.Services;
using DotNetEnv;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

Env.Load();
builder.Configuration.AddEnvironmentVariables();

var connectionString = builder.Configuration["MONGODB_CONNECTION_STRING"];
var dbName = builder.Configuration["MONGODB_DATABASE_NAME"];

if (string.IsNullOrWhiteSpace(connectionString) || string.IsNullOrWhiteSpace(dbName))
    throw new InvalidOperationException("MONGODB_CONNECTION_STRING or MONGODB_DATABASE_NAME is null");

builder.Services
    .AddIdentity<ApplicationUser, ApplicationRole>()
    .AddMongoDbStores<ApplicationUser, ApplicationRole, string>(connectionString, dbName)
    .AddDefaultTokenProviders();

builder.Services.Configure<MongoDbSettings>(options =>
{
    options.ConnectionString = connectionString;
    options.DatabaseName = dbName;
});

builder.Services.AddSingleton<IMongoDbService, MongoDbService>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IUserIdentifierService, UserIdentifierService>();

builder.Services.AddScoped<GameSetupService>();
builder.Services.AddScoped<BoardService>();
builder.Services.AddScoped<BoardSetupService>();
builder.Services.AddScoped<PieceSetupService>();
builder.Services.AddScoped<MovementPieceService>();
builder.Services.AddScoped<IGameTrackerService, GameTrackerService>();
builder.Services.AddScoped<IGameRulesService, GameRulesService>();

builder.Services.AddScoped<UserManager<ApplicationUser>>();
builder.Services.AddScoped<RoleManager<ApplicationRole>>();
builder.Services.AddScoped<SignInManager<ApplicationUser>>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

MongoDbMappings.RegisterClassMaps();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.Run();