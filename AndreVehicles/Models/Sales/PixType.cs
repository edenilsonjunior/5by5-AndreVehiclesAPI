namespace Models.Sales;


public class PixType
{
    public readonly static string INSERT = "INSERT INTO PixType(Name) VALUES(@Name); SELECT CAST(SCOPE_IDENTITY() AS INT);";

    public int Id { get; set; }
    public string Name { get; set; }

    public PixType() { }

    public PixType(string name)
    {
        Name = name;
    }
}

