using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.People;

public class Dependent : Person
{
    public static readonly string POST = "INSERT INTO Dependent(CustomerDocument, Document) VALUES (@CustomerDocument, @Document); ";

    public readonly static string GETALL = @"
    SELECT 
        p.Document,
        p.Name,
        p.BirthDate,
        p.Phone,
        p.Email,
        a.Id,
        a.Street,
        a.PostalCode,
        a.District,
        a.StreetType,
        a.AdditionalInfo,
        a.Number,
        a.State,
        a.City,
        d.CustomerDocument
    FROM 
        Person p
    JOIN 
        Address a ON p.AddressId = a.Id
    JOIN 
        Dependent d ON p.Document = d.Document ";

    public readonly static string GET = GETALL + " WHERE p.Document = @Document;";


    public Customer Customer { get; set; }
}
