using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models.People;


public class Address
{
    public readonly static string POST = "INSERT INTO Address (Id, Street, PostalCode, District, StreetType, AdditionalInfo, Number, State, City) VALUES (@Id, @Street, @PostalCode, @District, @StreetType, @AdditionalInfo, @Number, @State, @City);";

    public readonly static string GETALL = "SELECT Id, Street, PostalCode, District, StreetType, AdditionalInfo, Number, State, City FROM Address";

    public readonly static string GET = GETALL + " WHERE Id = @Id";

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Street { get; set; }
    public string PostalCode { get; set; }
    public string District { get; set; }
    public string StreetType { get; set; }
    public string AdditionalInfo { get; set; }
    public int Number { get; set; }
    public string State { get; set; }
    public string City { get; set; }

    public Address() { }

    public Address(string street, string postalCode, string district, string streetType, string additionalInfo, int number, string state, string city)
    {
        Street = street;
        PostalCode = postalCode;
        District = district;
        StreetType = streetType;
        AdditionalInfo = additionalInfo;
        Number = number;
        State = state;
        City = city;
    }
}
