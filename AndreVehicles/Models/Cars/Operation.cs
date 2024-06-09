namespace Models.Cars;


public class Operation
{
    public readonly static string POST = "INSERT INTO Operation(Description) VALUES(@Description); SELECT CAST(SCOPE_IDENTITY() AS INT);";
    public readonly static string GETALL = "SELECT Id, Description FROM Operation";
    public readonly static string GET = GETALL + " WHERE Id = @Id";


    public int Id { get; set; }
    public string Description { get; set; }

    public Operation() { }

    public Operation(string description)
    {
        Description = description;
    }
}
