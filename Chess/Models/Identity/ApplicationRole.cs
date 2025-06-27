using AspNetCore.Identity.MongoDbCore.Models;

public class ApplicationRole : MongoIdentityRole<string>
{
    public string Description { get; set; }
    public int AccessLevel { get; set; }
}