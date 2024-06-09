namespace Models.Sales;


public class Pix
{
    public readonly static string POST = "INSERT INTO Pix(Type, PixKey) VALUES(@Type, @PixKey); SELECT CAST(SCOPE_IDENTITY() AS INT);";
    public readonly static string GET = @"
    select 
        p.Id, 
        p.PixKey,
        pt.Id AS PixTypeId,
        pt.Name AS PixTypeName
    from 
    Pix p
    JOIN 
    PixType pt on p.Type = pt.Id
    WHERE 
        p.Id = @Id";

    public int Id { get; set; }
    public PixType Type { get; set; }
    public string PixKey { get; set; }

    public Pix() { }

    public Pix(PixType type, string pixKey)
    {
        Type = type;
        PixKey = pixKey;
    }
}

