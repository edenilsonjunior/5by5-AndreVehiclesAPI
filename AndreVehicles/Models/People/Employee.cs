namespace Models.People;


public class Employee : Person
{
    public readonly static string POST = "INSERT INTO Employee (Document, RoleId, CommissionValue, Commission) VALUES (@Document, @RoleId, @CommissionValue, @Commission);";
    public readonly static string GETALL = @"
    SELECT 
        p.Document,
        p.Name,
        p.BirthDate,
        p.Phone,
        p.Email,
        a.Street,
        a.PostalCode,
        a.District,
        a.StreetType,
        a.AdditionalInfo,
        a.Number,
        a.State,
        a.City,
        r.Description AS RoleDescription,
        e.CommissionValue,
        e.Commission
    FROM 
        Person p
    JOIN 
        Address a ON p.AddressId = a.Id
    JOIN 
        Employee e ON p.Document = e.Document
    JOIN 
        Role r ON e.RoleId = r.Id";

    public readonly static string GET = GETALL + " WHERE p.Document = @Document;";

    public Role Role { get; set; }
    public decimal CommissionValue { get; set; }
    public decimal Commission { get; set; }

    public Employee() { }

    public Employee(string document, string name, DateTime birthDate, Address address, string phone, string email, Role role, decimal commissionValue, decimal commission) : base(document, name, birthDate, address, phone, email)
    {
        Role = role;
        CommissionValue = commissionValue;
        Commission = commission;
    }

}
