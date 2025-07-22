using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using Chess.Abstractions.Services;

public class UserIdentifierService : IUserIdentifierService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserIdentifierService(
        IHttpContextAccessor httpContextAccessor,
        UserManager<ApplicationUser> userManager)
    {
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }

    public ObjectId GetUserObjectId()
    {
        var context = _httpContextAccessor.HttpContext;

        if (context?.User?.Identity?.IsAuthenticated == true)
        {
            var userIdStr = _userManager.GetUserId(context.User);
            if (ObjectId.TryParse(userIdStr, out var objectId))
                return objectId;

            throw new InvalidOperationException("Current user ID is not a valid ObjectId.");
        }

        return GetGuestId(context);
    }

    private ObjectId GetGuestId(HttpContext context)
    {
        const string cookieKey = "GuestId";

        if (context?.Request.Cookies.TryGetValue(cookieKey, out var cookieVal) == true &&
            ObjectId.TryParse(cookieVal, out var parsedCookieId))
        {
            return parsedCookieId;
        }

        var newGuestId = ObjectId.GenerateNewId();
        context?.Response.Cookies.Append(cookieKey, newGuestId.ToString(), new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTimeOffset.UtcNow.AddDays(7),
            Secure = true,
            IsEssential = true
        });

        return newGuestId;
    }
}
