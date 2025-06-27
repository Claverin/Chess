using AspNetCore.Identity.MongoDbCore.Models;

public class ApplicationUser : MongoIdentityUser<string>
{
    public string DisplayName { get; set; }
    public string Description { get; set; }
    public int EloRating { get; set; }
    public DateTime RegisteredAt { get; set; }
}