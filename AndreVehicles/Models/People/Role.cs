namespace Models.People;


public class Role
{
    public readonly static string INSERT = "INSERT INTO Role(Description) VALUES(@Description); SELECT CAST(SCOPE_IDENTITY() as INT);";

    public int Id { get; set; }
    public string Description { get; set; }

    public Role() { }

    public Role(string description)
    {
        Description = description;
    }
}
