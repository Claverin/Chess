using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using Chess.Models.Identity;

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

    public ObjectId CreateOrGetUserObjectId()
    {
        var context = _httpContextAccessor.HttpContext;
        var user = context?.User;

        if (user?.Identity?.IsAuthenticated == true)
        {
            var userIdStr = _userManager.GetUserId(user);
            if (ObjectId.TryParse(userIdStr, out var objectId))
                return objectId;
            throw new InvalidOperationException("Current user ID is corrupted (not a ObjectId)");
        }

        const string cookieKey = "PlayerId";

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
