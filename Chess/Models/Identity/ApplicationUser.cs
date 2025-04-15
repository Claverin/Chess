using AspNetCore.Identity.MongoDbCore.Models;

public class ApplicationUser : MongoIdentityUser<string>
{
    public string Description { get; set; }
}