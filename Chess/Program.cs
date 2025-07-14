using Chess.Abstractions.Services;
using Chess.Infrastructure;
using Chess.Intefaces.Infrastructure;
using Chess.Services;
using DotNetEnv;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

Env.Load();
builder.Configuration.AddEnvironmentVariables();

builder.Services.Configure<MongoDbSettings>(options =>
{
    options.ConnectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING");
    options.DatabaseName = Environment.GetEnvironmentVariable("MONGODB_DATABASE_NAME");
});

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddMongoDbStores<ApplicationUser, ApplicationRole, string>(
        builder.Configuration["MONGODB_CONNECTION_STRING"],
        builder.Configuration["MONGODB_DATABASE_NAME"]
    )
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IMongoDbService, MongoDbService>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IUserIdentifierService, UserIdentifierService>();

builder.Services.AddScoped<GameSetupService>();
builder.Services.AddScoped<BoardService>();
builder.Services.AddScoped<BoardSetupService>();
builder.Services.AddScoped<PieceSetupService>();
builder.Services.AddScoped<MovementPieceService>();
builder.Services.AddScoped<IGameTrackerService, GameTrackerService>();

builder.Services.AddScoped<UserManager<ApplicationUser>>();
builder.Services.AddScoped<RoleManager<ApplicationRole>>();
builder.Services.AddScoped<SignInManager<ApplicationUser>>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.Run();