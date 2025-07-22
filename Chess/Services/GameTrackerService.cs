using Microsoft.AspNetCore.Http;
using MongoDB.Bson;

public class GameTrackerService : IGameTrackerService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string CookieKey = "CurrentGameId";

    public GameTrackerService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void SetCurrentGameId(ObjectId gameId)
    {
        _httpContextAccessor.HttpContext?.Response.Cookies.Append(CookieKey, gameId.ToString(), new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            IsEssential = true,
            Expires = DateTimeOffset.UtcNow.AddDays(7)
        });
    }

    public ObjectId? GetCurrentGameId()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context?.Request.Cookies.TryGetValue(CookieKey, out var cookieValue) == true &&
            ObjectId.TryParse(cookieValue, out var gameId))
        {
            return gameId;
        }

        return null;
    }

    public void ClearCurrentGameId()
    {
        _httpContextAccessor.HttpContext?.Response.Cookies.Delete(CookieKey);
    }
}